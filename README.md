# MyLittlePony_Conexy — Pony Quiz (Mane 6 archetypes)

Небольшой fullstack‑проект: **психологически‑философский тест** в стилистике My Little Pony, который по ответам определяет наиболее подходящий архетип из **Mane 6**.

## Что внутри

- **Backend**: ASP.NET Core Web API + EF Core + PostgreSQL  
  - Авто‑миграции при старте
  - Сидирование данных (пони, вопросы, варианты ответов)
  - Swagger в Development
- **Frontend**: статическая страница `frontend/MyLittlePonyConexy.html`  
  - Пытается загрузить вопросы с API (`GET /api/quiz`)
  - Если API недоступно — использует локальный набор вопросов (fallback)

## Структура репозитория

- `backend/` — Web API + Dockerfile для бэкенда
- `MyLittlePony_Conexy/` — исходники .NET проекта (также содержит сидер)
- `frontend/` — HTML‑страница для тестирования
- `docker-compose.yml` — Postgres + backend + frontend

## Быстрый старт (Docker Compose)

Требования: установлен Docker Desktop.

```bash
docker compose up --build
```

Что поднимется:
- **Postgres** (в контейнере `db`)
- **Backend** (контейнер `backend`, переменная `SeedData=true` включает сидирование)
- **Frontend** (контейнер `frontend`, отдаёт статику на `http://localhost:8080`)

## Запуск backend локально (без Docker)

Требования: .NET SDK, PostgreSQL.

1) Подготовьте строку подключения.

Можно через `appsettings.json`:
- `backend/MyLittlePony_Conexy/appsettings.json` → `ConnectionStrings:DefaultConnection`

Или через переменные окружения:
- `CONNECTION_STRING` (имеет приоритет в `backend/MyLittlePony_Conexy/Program.cs`)
- либо стандартный для .NET вариант: `ConnectionStrings__DefaultConnection`

2) Запустите проект (один из вариантов):

```bash
dotnet run --project backend/MyLittlePony_Conexy/MyLittlePony_Conexy.csproj
```

При старте:
- применятся миграции (`Database.Migrate()`)
- при `SeedData=true` база заполнится начальными данными (включая **15 вопросов**)

## API

Базовые эндпоинты:

- **Получить вопросы**: `GET /api/quiz`  
  Возвращает массив вопросов с вариантами (id + text).

- **Посчитать результат**: `POST /api/quiz/result`

Пример тела запроса:

```json
{
  "selectedOptionIds": [1, 5, 9]
}
```

Ответ:
- `pony` (id, name, description, imageUrl, traits, totalScore)

## Тестирование через HTML

Откройте файл:
- `frontend/MyLittlePonyConexy.html`

Поведение:
- если доступен backend на том же origin — страница загрузит вопросы через API и отправит результаты на сервер
- если backend недоступен — включится локальный fallback (в нём также **15 вопросов**)

## Сидирование вопросов

Файл с вопросами:
- `MyLittlePony_Conexy/Infrastructure/Seed/QuizSeeder.cs`
- (дубликат для сборки в `backend/`): `backend/MyLittlePony_Conexy/Infrastructure/Seed/QuizSeeder.cs`

Вопросы/варианты связаны с Mane 6 через `PonyWeights`.

## Лицензия

Проект учебный/демо. .

