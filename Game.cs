using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using dataCentre.server;

namespace dataCentre
{
    public class Game
    {
        // Общий объект синхронизации вывода, чтобы фоновые сообщения
        // не перемешивались с командами игрока.
        private readonly object _consoleLock = new();

        // Храним источник отмены, чтобы любое событие (команда exit или GameOver)
        // могло единообразно завершить и фоновые задачи, и чтение из консоли.
        private CancellationTokenSource? _shutdownSource;

        public async Task GoGame()
        {
            using CancellationTokenSource cts = new();
            _shutdownSource = cts;
            Commands com = new Commands(_consoleLock);

            // Стартуем асинхронную фоновую задачу непосредственно перед циклом чтения команд,
            // чтобы она работала параллельно с игровыми действиями.
            Task backgroundTask = Task.Run(() => BackgroundLoopAsync(cts.Token));
            while (true)
            {
                string? command;
                try
                {
                    // Читаем команды асинхронно с поддержкой отмены, чтобы GameOver мгновенно
                    // останавливал ввод, даже если пользователь не нажал Enter.
                    command = await Console.In.ReadLineAsync(cts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                bool shouldExit = command is null || com.readCommand(command);

                if (shouldExit)
                {
                    // При завершении игры сигнализируем фоновой задаче о необходимости остановки.
                    SignalShutdown();
                    break;
                }

            }

            // Повторно запрашиваем отмену (если цикл завершился не по команде, а по GameOver),
            // затем дожидаемся фоновой задачи для корректного завершения.
            SignalShutdown();

            try
            {
                await backgroundTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _shutdownSource = null;
            }
        }

        private async Task BackgroundLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    // Ждём 15 секунд между проверками, сохраняя возможность отмены.
                    await Task.Delay(TimeSpan.FromSeconds(15), token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                if (token.IsCancellationRequested)
                {
                    break;
                }

                // Здесь можно вызывать любую «тяжёлую» игровую логику, требующую регулярного запуска.
                foreach (Server server in EnumerateServers())
                {
                    server.testWork();
                }

                if (CheckForGameOver())
                {
                    lock (_consoleLock)
                    {
                        Console.WriteLine("[Background] Game over: все серверы вышли из строя.");
                    }

                    // Завершаем игру, чтобы остановить цикл чтения команд и сам фон.
                    SignalShutdown();
                    break;
                }

                lock (_consoleLock)
                {
                    // Сообщаем пользователю о завершении фоновой работы.
                    Console.WriteLine("[Background] Server diagnostics completed.");
                }
            }
        }

        private void SignalShutdown()
        {
            // Метод используется и основным циклом, и фоновыми событиями, чтобы гарантировать,
            // что отмена будет запрошена ровно один раз и безопасно.
            CancellationTokenSource? source = _shutdownSource;
            if (source is not null && !source.IsCancellationRequested)
            {
                source.Cancel();
            }
        }

        private static IEnumerable<Server> EnumerateServers()
        {
            yield return AllServers.server1;
            yield return AllServers.server2;
            yield return AllServers.server3;
            yield return AllServers.server4;
            yield return AllServers.server5;
        }

        private static bool CheckForGameOver()
        {
            // Игра считается проигранной, когда все серверы достигли статуса Failed.
            return EnumerateServers().All(server => server.status == ServerStatus.Failed);
        }
    }
}
