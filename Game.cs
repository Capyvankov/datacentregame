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

        private CancellationTokenSource? _cts;
        private bool _isGameOver;

        public async Task GoGame()
        {
            _cts = new CancellationTokenSource();
            _isGameOver = false;
            Commands com = new Commands(_consoleLock);

            void OnGameOverTriggered()
            {
                _isGameOver = true;
                _cts?.Cancel();
            }

            GameOver.Reset();
            GameOver.GameOverTriggered += OnGameOverTriggered;

            Task backgroundTask = Task.Run(() => BackgroundLoopAsync(_cts.Token));

            try
            {
                while (!_isGameOver)
                {
                    if (Console.KeyAvailable)
                    {
                        string? command = Console.ReadLine();
                        bool shouldExit = command is null || com.readCommand(command);

                        if (shouldExit)
                        {
                            _isGameOver = true;
                            _cts?.Cancel();
                        }
                    }
                    else
                    {
                        try
                        {
                            await Task.Delay(50, _cts.Token).ConfigureAwait(false);
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                    }
                }
            }
            finally
            {
                GameOver.GameOverTriggered -= OnGameOverTriggered;

                if (_cts is { IsCancellationRequested: false })
                {
                    _cts.Cancel();
                }

                try
                {
                    await backgroundTask.ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                }

                _cts.Dispose();
                _cts = null;
            }
        }

        private async Task BackgroundLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    // Ждём 15 секунд между проверками, сохраняя возможность отмены.
                    await Task.Delay(TimeSpan.FromSeconds(3), token).ConfigureAwait(false);
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
