## Базовий об’єм завдання.
Необхідно реалізувати RESTful WebAPI використовуючи платформу .Net 8 версії або вище. Технологія зберігання даних довільна (бажано MongoDB). В рамках реалізації необхідно забезпечити ізоляцію/абстагувати для транспортного, логічного прошарку та шару зберігання даних (для змоги замінити реалізацію протоколу обміну даними чи реалізацію бази даних). Не бажано використовувати бібліотеку MediatR.

### Необхідний фукціонал: 
- Користувач має змогу керувати списками задач.
- Користувач може поділитися списком задач з іншим користувачем додавши відповідний зв’язок.
- Ідентифікатор поточного користувача має передаватися разом із запитом.
-  Систему аутентифікації та авторизації реалізовувати не потрібно.

### Обов’язкові атрибути для сутності “Список задач”:
- Назва списку, довжина від 1 до 255 символів
- Користувач, який створив список (Власник)

### Необхідні дії зі списком задач:
- Створити новий список задач
- Змінити існуючий список задач
- Видалити існуючий список задач
- Отримати один існуючий список задач
- Отримати список списків задач (достатньо отримати обмежений об’єм даних по спискам, наприклад тільки ідентифікатор та назву списку). Має містити сортування по часу створення (від нових до старих) та пагинацію.
- Додати зв’язок одного списку задач з вказаним користувачем
- Отримати зв’язки одного списку задач з користувачами
- Прибрати зв’язок одного списку задач з вказаним користувачем

### Правила обмеження роботи з існуючими списками:
- При отримані списку списків задач необхідно повертати тільки ті сутності, де вказаний у запиті користувач, або є власником списку або має зв’язок зі списком задач. 
- Отримати список задач, Змінити список задач, Додати зв’язок списку задач з вказаним користувачем, Отримати зв’язки списку задач з користувачами, Прибрати зв’язок списку задач з вказаним користувачем може власник списку або користувач, що має зв’язок зі списком задач. 
- Видалити список задач може тільки власник списку.
