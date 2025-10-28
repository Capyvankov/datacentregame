using dataCentre;
using Spectre.Console;

Game game = new();

// Точка входа теперь асинхронная: ждём завершения игрового цикла, включая фоновые задачи.
await game.GoGame();

//Printer.SystMes("123", "red", "ERROR");
