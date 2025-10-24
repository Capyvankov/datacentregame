# dataCentre Game

## Получение проекта

На данный момент репозиторий существует только локально. Чтобы загрузить его на GitHub и получить доступ из любого места:

1. Создайте новый пустой репозиторий на GitHub.
2. Добавьте удалённый origin в локальный репозиторий:
   ```bash
   git remote add origin git@github.com:<your-account>/<your-repo>.git
   ```
   или используйте HTTPS:
   ```bash
   git remote add origin https://github.com/<your-account>/<your-repo>.git
   ```
3. Запушьте текущую ветку (по умолчанию `work`) вместе с историей:
   ```bash
   git push -u origin work
   ```
4. После этого репозиторий будет доступен на GitHub. Склонировать его можно командой:
   ```bash
   git clone git@github.com:<your-account>/<your-repo>.git
   ```

Если у вас нет доступа к GitHub, можно создать архив:

```bash
zip -r datacentregame.zip .
```

и передать файл любым удобным способом.
