# Authorization and Table of users

 Web application that allows users to register and authenticate.

- **Unauthenticated** users do not have access to user management (can only access the registration form or the authentication form).
- **Authenticated** users see a table (table with id, name, email, registration date, last login date, status) with users.
- The table in the left column contains checkboxes for multiple selection, in the column header there is a checkbox "select all/deselect". Above the table *toolbar* with actions: Block (button with text), Unblock (icon), Delete (icon).
- The user can delete or block himself - at the same time he must be logged out immediately. If someone else blocks or deletes the user, any next action will redirect the user to the login page.
- While registrating it is possible to use any password, even one character.

_________________________________________________________________________________________________________________________________________________________________________

Web-приложение, которое позволяет пользователям зарегистрироваться и аутентифицироваться.

- **Неаутентифицированные** пользователи не имеют доступа к управлению пользователями (могут достучаться только к форме регистрации или форме аутентификации).
- **Аутентифицированные** пользователи видят таблицу (таблица с идентификатором, именем, email-ом, датой регистрации, датой последнего логина, статусом) с пользователями. 
- Таблица левой колонкой содержит чек-боксы для множественного выделения, в заголовке колонки чек-бокс "выделить все/снять выделение". Над таблицей *тулбар* с действиями: Block (кнопка с текстом), Unblock (иконка), Delete (иконка). 
- Пользователь может удалить или заблокировать себя — при этом сразу должен быть разлогинен. Если кто-то другой блокирует или удаляет пользователя, то при любом следующем действии пользователь переправляется на страницу логина.
- При регистрации есть возможность использовать любой пароль, даже из одного символа.
