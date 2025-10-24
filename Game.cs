using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dataCentre
{
    public class Game
    {
        // Общий объект синхронизации вывода, чтобы фоновые сообщения
        // не перемешивались с командами игрока.
        private readonly object _consoleLock = new();

        public async Task GoGame()
        {
            using CancellationTokenSource cts = new();
            Commands com = new Commands(_consoleLock);

            // Стартуем асинхронную фоновую задачу непосредственно перед циклом чтения команд,
            // чтобы она работала параллельно с игровыми действиями.
            Task backgroundTask = Task.Run(() => BackgroundLoopAsync(cts.Token));
            while (true)
            {
                string? command = Console.ReadLine();
                bool shouldExit = command is null || com.readCommand(command);

                if (shouldExit)
                {
                    // При завершении игры сигнализируем фоновой задаче о необходимости остановки.
                    cts.Cancel();

                    try
                    {
                        // Дожидаемся завершения фонового цикла, чтобы корректно освободить ресурсы.
                        await backgroundTask.ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                    }

                    break;
                }

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
                EventGen.Event();
                
                lock (_consoleLock)
                {
                    // Сообщаем пользователю о завершении фоновой работы.
                    //Console.WriteLine("[Background] Server diagnostics completed.");
                }
            }
        }
    }
}
