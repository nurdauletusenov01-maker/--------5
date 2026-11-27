# ASP.NET Core Identity — Студенттер жүйесі

## Мақсаты
Бұл ASP.NET Core MVC жобасында Identity арқылы тіркелу, кіру, шығу, Admin және Student рөлдері және студенттер тізімін қорғау жүзеге асырылған.

## Технологиялар
- .NET 8
- ASP.NET Core MVC
- C#
- ASP.NET Core Identity
- Entity Framework Core
- SQLite

## Орнату
```powershell
dotnet restore
dotnet tool install --global dotnet-ef --version 8.0.22
dotnet ef database update
dotnet run
```

## Конфигурация және құпия деректер
SQLite дерекқорының файл атауы `appsettings.json` ішінде көрсетілген. Құпия парольдер репозиторийге жазылмайды. Қосымша Development Admin-ді тек User Secrets арқылы жасауға болады:

```powershell
dotnet user-secrets init
dotnet user-secrets set "Identity:AdminEmail" "admin@example.com"
dotnet user-secrets set "Identity:AdminPassword" "<құпиясөзіңіз>"
dotnet run
```

`<құпиясөзіңіз>` орнына жеке құпиясөзіңізді қойыңыз; нақты құпиясөзді README-ге немесе репозиторийге жазбаңыз. Admin құпиясөзін User Secrets арқылы беріңіз. Әрбір жаңа тіркелуші автоматты түрде `Student` рөлін алады.

## Рөлдер
- **Student:** студенттер тізімін көреді; пайдаланушыны жоя алмайды.
- **Admin:** студенттер тізімін көреді және Student пайдаланушыларын жоя алады.

## Тестілеу нәтижелері
1. Бос және дұрыс емес деректер тіркелуден бас тартылады.
2. Қайталанған Email қабылданбайды.
3. Жаңа пайдаланушыға `Student` рөлі тағайындалады.
4. Кіремеген пайдаланушы `/Students` бетіне кіре алмайды.
5. Student тізімді көре алады, Delete батырмасы көрсетілмейді.
6. Admin тізімді көре алады және Student жазбаларын жояды.
7. Logout тек POST арқылы орындалады.
8. Құпия құпия сөздер кодқа немесе GitHub репозиторийіне жазылмайды.

## Скриншоттар
Нақты жұмыс күйіндегі скриншоттар:
- `register.png`
- `login.png`
- `student.png`
- `admin.png`
