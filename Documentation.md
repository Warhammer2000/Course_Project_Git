# 📚 Техническая документация проекта CourseProjectItems

## 1. Общая информация

### Заголовок и Описание

**CourseProjectItems** – это веб-приложение для управления коллекциями и элементами внутри этих коллекций. Пользователи могут создавать, редактировать, удалять и просматривать коллекции и элементы, оставлять комментарии и лайки.

### Цель

Цель проекта – предоставить пользователям удобный интерфейс для управления личными коллекциями различных элементов, а также обеспечить возможность взаимодействия между пользователями через комментарии и лайки.

### Технологии

- **ASP.NET Core MVC**
- **C#**
- **Entity Framework Core**
- **Microsoft SQL Server**
- **ASP.NET Identity**
- **Cloudinary (для хранения изображений)**
- **Bootstrap**
- **Newtonsoft.Json**
- **CsvHelper**
- **SendGrid**

## 2. Структура проекта

### Архитектура

Проект основан на архитектуре MVC (Model-View-Controller), где:

- **Model**: Представляет данные приложения и бизнес-логику.

- **View**: Отвечает за отображение данных пользователю.

- **Controller**: Обрабатывает пользовательские запросы и взаимодействует с моделью для подготовки данных для отображения.

  ### 💡 **Диаграммы и схемы:**

  **Диаграмма активности:** 

  https://drive.google.com/uc?export=view&id=16SIe39NC4RtdHBIXZaKHNiwtExaDOx19	

  **Диаграмма развертывания:**

  https://drive.google.com/uc?export=view&id=10LK96P6mKmYdChQJ_zzoKfJKVrj7z-LQ

  **Диаграмма последовательностей:**
  
  https://drive.google.com/uc?export=view&id=1BCQEMb4IJzEBVc3wigzZ31GXyXRlyNCA
  
  **Диаграмма компонентов:**
  
  https://drive.google.com/uc?export=view&id=1E93rQhsZkLysX9UrQsKaM5mioGEe0od0
  
  **Диаграмма классов:**
  
  https://drive.google.com/uc?export=view&id=1U3t6iCaPrfMZPazKhTuouKFnUk4yXnvb
  
  Ссылка на папку с UML Диаграммами : 
  
  https://drive.google.com/drive/folders/1jhvONmUsNeh0DSpSKJU_aHmEg_S9F2vH?usp=sharing
  
  

### Модули и Компоненты

- **Controllers**: Обработка HTTP-запросов и управление потоком данных между моделью и представлением.
- **Models**: Определение структур данных и взаимодействие с базой данных.
- **ViewModels**: Объекты, используемые для передачи данных между контроллерами и представлениями.
- **Repositories**: Обеспечение абстракции для доступа к данным.
- **Services**: Логика бизнес-процессов и взаимодействие с внешними сервисами.
- **Views**: Представления, отображающие данные пользователю.

## 3. Установка и настройка

### Системные Требования

- **Операционная система**: Windows 10 или выше
- **RAM**: Минимум 4 ГБ (рекомендуется 8 ГБ)
- **Процессор**: Dual Core 2 ГГц или выше
- **.NET SDK**: .NET 6.0 или выше
- **База данных**: Microsoft SQL Server 2019 или выше

### Установка

1. **Установите .NET SDK**: Скачайте и установите последнюю версию .NET SDK с официального сайта [.NET](https://dotnet.microsoft.com/download).
2. **Установите Microsoft SQL Server**: Скачайте и установите Microsoft SQL Server. Убедитесь, что установлены все необходимые компоненты, включая Full-Text Search.
3. **Клонируйте репозиторий**: `git clone https://github.com/username/CourseProjectItems.git`
4. **Перейдите в папку проекта**: `cd CourseProjectItems`
5. **Восстановите зависимости**: `dotnet restore`

### Конфигурация

1. Настройте appsettings.json

   : Откройте файл 

   ```
   appsettings.json
   ```

    и настройте строки подключения к базе данных и другие параметры.

   ```
   jsonКопировать код{
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=CourseProjectDB;Trusted_Connection=True;"
     },
     "CloudinarySettings": {
       "CloudName": "your_cloud_name",
       "ApiKey": "your_api_key",
       "ApiSecret": "your_api_secret"
     }
   }
   ```

## 4. Разработка и кодирование

### Кодовая Структура

- **/Controllers**: Контроллеры приложения
- **/Models**: Модели данных
- **/ViewModels**: ViewModel для передачи данных
- **/Repositories**: Репозитории для доступа к данным
- **/Services**: Сервисы приложения
- **/Views**: Представления приложения

### Соглашения по Кодированию

- **Именование**: PascalCase для классов и методов, camelCase для переменных и параметров.
- **Форматирование**: Используйте стандартные отступы и форматирование кода, принятые в C#.
- **Комментарии**: Документируйте все публичные методы и классы.

## Описание Классов и Методов

## Controller :

- - **AccountController.cs:**

    - `Login(string returnUrl = null)`: Отображает страницу входа.
    - `Login(LoginViewModel loginViewModel, string returnUrl = null)`: Обрабатывает вход пользователя.
    - `Register()`: Отображает страницу регистрации.
    - `Register(RegisterViewModel registerViewModel)`: Обрабатывает регистрацию нового пользователя.
    - `Logout()`: Выполняет выход пользователя.
    - `UserPage(CollectionType? type, string id, string name = "default", int page = 1, SortState sortOrder = SortState.NameAsc)`: Отображает страницу пользователя с фильтрацией и сортировкой коллекций.
    - `RegisterConfirmation(string email)`: Отображает подтверждение регистрации.
    - `ConfirmEmail(string userId, string code)`: Подтверждает электронную почту пользователя.
  
    **AdminController.cs:**
  
    - `Index()`: Возвращает список всех пользователей.
    - `DeleteUser(string userId)`: Удаляет пользователя.
    - `BlockUser(string userId)`: Блокирует пользователя.
    - `LockUser(string userId)`: Блокирует пользователя на определенный период.
    - `UnblockUser(string userId)`: Разблокирует пользователя.
    - `UnlockUser(string userId)`: Снимает блокировку с пользователя.
    - `AddAdmin(string userId)`: Назначает пользователя администратором.
    - `RemoveAdmin(string userId)`: Снимает права администратора с пользователя.
  
    **CollectionsController.cs:**
  
    - `Index()`: Отображает список всех коллекций.
    - `Details(int id)`: Отображает детали конкретной коллекции.
    - `Create()`: Отображает форму создания коллекции.
    - `CreateItem(int collectionId)`: Перенаправляет на страницу создания элемента.
    - `Create(CollectionViewModel viewModel)`: Создает новую коллекцию.
    - `Edit(int id)`: Отображает форму редактирования коллекции.
    - `Edit(int id, CollectionViewModel viewModel)`: Обновляет данные коллекции.
    - `Delete(int id)`: Отображает форму удаления коллекции.
    - `DeleteConfirmed(int id)`: Удаляет коллекцию и связанные элементы.
    - `ExportToCsv(int id)`: Экспортирует данные коллекции в CSV-файл.
  
    **CommentsController.cs:**
  
    - `AddComment(Comment comment)`: Добавляет новый комментарий.
    - `DeleteComment(int id)`: Удаляет комментарий.
  
    **HomeController.cs:**
  
    - `Index(string searchString)`: Отображает главную страницу с возможностью поиска.
    - `SetLanguage(string culture, string returnUrl)`: Устанавливает язык интерфейса.
    - `Privacy()`: Отображает страницу политики конфиденциальности.
    - `Error()`: Обрабатывает ошибки и отображает страницу ошибки.
  
    **ItemsController.cs:**
  
    - `Index()`: Отображает список всех элементов.
    - `Details(int id)`: Отображает детали конкретного элемента.
    - `Create(int collectionId)`: Отображает форму создания элемента.
    - `Create(ItemViewModel viewModel)`: Создает новый элемент.
    - `Edit(int id)`: Отображает форму редактирования элемента.
    - `Edit(int id, ItemViewModel viewModel)`: Обновляет данные элемента.
    - `Delete(int id)`: Отображает форму удаления элемента.
    - `DeleteConfirmed(int id)`: Удаляет элемент и связанные данные.
    - `AddComment(int itemId, CommentViewModel commentViewModel)`: Добавляет новый комментарий к элементу.
    - `DeleteComment(int id)`: Удаляет комментарий к элементу.
  
    **LikesController.cs:**
  
    - `Like(int itemId)`: Добавляет лайк к элементу.
    - `Unlike(int itemId)`: Удаляет лайк с элемента.
  
    **PhotosController.cs:**
  
    - `UploadPhoto(IFormFile file)`: Загружает фото.
    - `DeletePhoto(string publicId)`: Удаляет фото.
  
    ## Interfaces
  
    **IPhotoService.cs:**
  
    - `Task<ImageUploadResult> AddPhotoAsync(IFormFile file)`: Добавляет фото.
    - `Task<DeletionResult> DeletePhotoAsync(string publicUrl)`: Удаляет фото.
  
    **ICollectionItem.cs:**
  
    - `string Name { get; set; }`: Имя элемента коллекции.
    - `CollectionType Type { get; set; }`: Тип коллекции.
  
    **ICollectionRepository.cs:**
  
    - `Task<IEnumerable<Collection>> GetCollectionsByUserId(string userId)`: Получить коллекции по ID пользователя.
  
    **ICollectionService.cs:**
  
    - `Task<IEnumerable<Collection>> GetAllCollections()`: Получить все коллекции.
    - `Task<Collection> GetCollectionById(int id)`: Получить коллекцию по ID.
    - `Task AddCollection(Collection collection)`: Добавить коллекцию.
    - `Task UpdateCollection(Collection collection)`: Обновить коллекцию.
    - `Task DeleteCollection(int id)`: Удалить коллекцию.
    - `Task<IEnumerable<Collection>> GetCollectionsByUserId(string userId)`: Получить коллекции по ID пользователя.
  
    **ICommentRepository.cs:**
  
    - `Task<IEnumerable<Comment>> GetCommentsByItemId(int itemId)`: Получить комментарии по ID элемента.
  
    **IEmailSender.cs:**
  
    - `Task SendEmailAsync(string email, string subject, string message)`: Отправить электронное письмо.
  
    **IGenericRepository.cs:**
  
    - `Task<IEnumerable<T>> GetAll()`: Получить все записи.
    - `Task<T> GetById(int id)`: Получить запись по ID.
    - `Task Add(T entity)`: Добавить запись.
    - `Task Update(T entity)`: Обновить запись.
    - `Task Delete(int id)`: Удалить запись.
    - `IQueryable<T> Find(Expression<Func<T, bool>> expression)`: Найти записи по выражению.
  
    **ILikeRepository.cs:**
  
    - `Task<bool> IsLikedByUser(int itemId, string userId)`: Проверить, поставил ли пользователь лайк элементу.
  
    ## Services
  
    **CollectionService.cs:**
  
    - `Task<IEnumerable<Collection>> GetAllCollections()`: Получить все коллекции.
    - `Task<Collection> GetCollectionById(int id)`: Получить коллекцию по ID.
    - `Task AddCollection(Collection collection)`: Добавить коллекцию.
    - `Task UpdateCollection(Collection collection)`: Обновить коллекцию.
    - `Task DeleteCollection(int id)`: Удалить коллекцию.
    - `Task<IEnumerable<Collection>> GetCollectionsByUserId(string userId)`: Получить коллекции по ID пользователя.
  
    **PhotoService.cs:**
  
    - `Task<ImageUploadResult> AddPhotoAsync(IFormFile file)`: Добавляет фото.
    - `Task<DeletionResult> DeletePhotoAsync(string publicUrl)`: Удаляет фото.
  
    **EmailSender.cs:**
  
    - `Task SendEmailAsync(string email, string subject, string message)`: Отправляет электронное письмо.
  
    ## Repository
  
    **LikeRepository.cs:**
  
    - `Task<bool> IsLikedByUser(int itemId, string userId)`: Проверить, поставил ли пользователь лайк элементу.
  
    **CollectionRepository.cs:**
  
    - `Task<IEnumerable<Collection>> GetCollectionsByUserId(string userId)`: Получить коллекции по ID пользователя.
  
    **CommentRepository.cs:**
  
    - `Task<IEnumerable<Comment>> GetCommentsByItemId(int itemId)`: Получить комментарии по ID элемента.
  
    **GenericRepository.cs:**
  
    - `Task<IEnumerable<T>> GetAll()`: Получить все записи.
    - `Task<T> GetById(int id)`: Получить запись по ID.
    - `Task Add(T entity)`: Добавить запись.
    - `Task Update(T entity)`: Обновить запись.
    - `Task Delete(int id)`: Удалить запись.
    - `IQueryable<T> Find(Expression<Func<T, bool>> expression)`: Найти записи по выражению.

## 5. Базы данных

### Модель данных

- **Users**: Таблица пользователей.
- **Collections**: Таблица коллекций.
- **Items**: Таблица элементов коллекций.
- **Comments**: Таблица комментариев.
- **Likes**: Таблица лайков.

### Миграции

- Для применения миграций используйте команды:

  ```
  bashКопировать кодdotnet ef migrations add InitialCreate
  dotnet ef database update
  ```

### Примеры Запросов

- Получение всех коллекций:

  ```
  csharp
  Копировать код
  var collections = _context.Collections.ToList();
  ```

## 6. Тестирование

### Типы Тестов

- **Юнит-тесты**: Проверка отдельных методов и классов.
- **Интеграционные тесты**: Проверка взаимодействия между компонентами.

### Среда Тестирования

- **xUnit**: Фреймворк для тестирования.

### Инструкции по Запуску Тестов

- Для запуска тестов используйте команду:

  ```
  dotnet test
  ```

## 7. Развертывание

### Процесс Развертывания

1. **Сборка проекта**: `dotnet build`
2. **Применение миграций**: `dotnet ef database update`
3. **Публикация проекта**: `dotnet publish -c Release -o ./publish`

### Среда Развертывания

- **Azure**

### CI/CD

- Настройте автоматическое развёртывание с использованием  **Jenkins**.

## 8. Поддержка и сопровождение

### Мониторинг

- **Application Insights**: Для мониторинга и логирования.

### Логи и Ошибки

- Логи хранятся в файле `logs.txt` в корне проекта.

### Обновления

- Для выпуска обновлений используйте `dotnet publish` и примените обновления на сервере.

## 9. Примеры использования

### Пользовательские Сценарии

1. **Регистрация и вход**: Пользователь регистрируется и входит в систему.
2. **Создание коллекции**: Пользователь создаёт новую коллекцию.
3. **Добавление элемента**: Пользователь добавляет элемент в коллекцию.
4. **Оставление комментария**: Пользователь оставляет комментарий к элементу.
5. **Лайк элемента**: Пользователь ставит лайк элементу.

### API Документация

- Для доступа к API используйте Swagger: `/swagger`

## 10. Приложения

### Полезные Ссылки

- [Документация ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
- [Документация Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

### Часто Задаваемые Вопросы (FAQ)

- Как настроить соединение с базой данных?
  - Настройте строку подключения в `appsettings.json`.
- Как применить миграции?
  - Используйте команду `dotnet ef database update`.

------

✨ Спасибо за использование CourseProjectItems! Я надеюсь, что вам понравится моё приложение. ✨