# Master GYM — Трислойно приложение

## Описание
Приложение за управление на фитнес зала, разработено като трислойна архитектура в C# / .NET 8.

---

## Структура на проекта

```
GymApp/
├── GymApp.sln
├── GymApp.Data/              ← Слой за данни
│   ├── Models/               ← Entity класове
│   │   ├── Member.cs
│   │   ├── Trainer.cs
│   │   ├── Workout.cs
│   │   ├── Exercise.cs
│   │   ├── WorkoutExercise.cs
│   │   ├── MemberWorkout.cs
│   │   └── Subscription.cs
│   └── Context/
│       └── GymDbContext.cs   ← Entity Framework DbContext
│
├── GymApp.Services/          ← Слой за услуги (бизнес логика)
│   ├── Interfaces/
│   │   ├── IMemberService.cs
│   │   ├── ITrainerService.cs
│   │   ├── IWorkoutService.cs
│   │   ├── IExerciseService.cs
│   │   └── ISubscriptionService.cs
│   └── Implementations/
│       ├── MemberService.cs
│       ├── TrainerService.cs
│       ├── WorkoutService.cs
│       ├── ExerciseService.cs
│       └── SubscriptionService.cs
│
├── GymApp.ConsoleUI/         ← Презентационен слой (конзолен интерфейс)
│   ├── Program.cs
│   ├── ConsoleHelper.cs
│   └── Menus/
│       ├── MembersMenu.cs
│       ├── TrainersMenu.cs
│       ├── WorkoutsMenu.cs
│       ├── ExercisesMenu.cs
│       └── SubscriptionsMenu.cs
│
└── GymApp.Tests/             ← Компонентни тестове (NUnit)
    ├── TestBase.cs
    ├── MemberServiceTests.cs
    ├── TrainerServiceTests.cs
    ├── WorkoutServiceTests.cs
    ├── ExerciseServiceTests.cs
    └── SubscriptionServiceTests.cs
```

---

## Технологии

| Технология | Версия | Цел |
|---|---|---|
| .NET | 8.0 | Основна платформа |
| Entity Framework Core | 8.0 | ORM за база данни |
| Pomelo.EFCore.MySql | 8.0 | MySQL провайдър за EF |
| Microsoft.Extensions.DependencyInjection | 8.0 | Dependency Injection |
| NUnit | 4.1 | Unit тестове |
| EFCore.InMemory | 8.0 | In-memory DB за тестове |
| Moq | 4.20 | Mocking за тестове |

---

## База данни

Приложението използва MySQL база данни **gym** с 7 таблици:

- **members** — членове на фитнес залата
- **trainers** — треньори
- **workouts** — тренировъчни програми
- **exercises** — упражнения
- **workout_exercises** — упражнения в тренировки (M:N)
- **members_workouts** — записвания за тренировки (M:N)
- **subscriptions** — абонаменти (Monthly / Annual)

---

## Инсталация и стартиране

### 1. Изисквания
- .NET 8 SDK
- MySQL Server (5.7+ или 8.x)
- Visual Studio 2022 / VS Code / Rider

### 2. Настройка на базата данни
```sql
-- Изпълнете Master_GYM.sql в MySQL Workbench или CLI:
mysql -u root -p < Master_GYM.sql
```

### 3. Настройка на connection string
Отворете `GymApp.ConsoleUI/Program.cs` и редактирайте:
```csharp
string connectionString = "Server=localhost;Database=gym;User=root;Password=ВАШАТА_ПАРОЛА;";
```

### 4. Стартиране
```bash
cd GymApp/GymApp.ConsoleUI
dotnet run
```

### 5. Стартиране на тестовете
```bash
cd GymApp/GymApp.Tests
dotnet test
```

---

## Функционалности

### Членове
- Преглед на всички членове
- Търсене по ID
- Добавяне / Редактиране / Изтриване
- Филтриране на активни членове (с валиден абонамент)
- Преглед на тренировки на член

### Треньори
- Пълен CRUD
- Преглед на тренировки на треньор

### Тренировки
- Пълен CRUD
- Филтриране по ниво на трудност (Easy / Medium / Hard)
- Детайлен преглед с упражнения

### Упражнения
- Пълен CRUD
- Филтриране по мускулна група

### Абонаменти
- Пълен CRUD
- Преглед само на активни абонаменти
- Преглед по конкретен член

---

## Архитектурни решения

**Трислойна архитектура:**
1. **Data Layer** — Entity Framework модели и DbContext; не съдържа бизнес логика
2. **Service Layer** — Всяка услуга имплементира интерфейс; лесна за замяна и тестване
3. **Presentation Layer** — Конзолен UI, ползва само Service Layer; не знае нищо за базата данни

**Dependency Injection** — чрез `Microsoft.Extensions.DependencyInjection`; всички зависимости се инжектират, не се инстанцират ръчно.

**Тестове** — използват in-memory база данни (EFCore.InMemory) за пълна изолация; тестват реалните имплементации без мокване на EF.

---

## Покритие с тестове

| Клас | Брой тестове |
|---|---|
| MemberService | 9 |
| TrainerService | 8 |
| WorkoutService | 9 |
| ExerciseService | 9 |
| SubscriptionService | 10 |
| **Общо** | **45** |

Покритие: **> 80%** от Service Layer кода.

---

## Автори
Разработено от екип — курсов проект по C# / .NET
