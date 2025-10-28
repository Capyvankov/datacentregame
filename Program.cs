using dataCentre;

Game game = new();

// Точка входа теперь асинхронная: ждём завершения игрового цикла, включая фоновые задачи.
await game.GoGame();
