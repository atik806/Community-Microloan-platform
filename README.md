# Community Microloan Platform

A Windows Forms desktop app for running a small community microloan scheme. Borrowers
sign up and request loans, lenders approve or reject them, monitors vet new borrowers,
helpers register borrowers on someone else's behalf, and an admin manages every account
and watches the shared fund.

Built with C# (.NET Framework 4.7.2, WinForms) on SQL Server.

## Roles

| Role     | What they do |
|----------|--------------|
| Borrower | Signs up (pending until a monitor approves), requests a loan from a chosen lender, repays via bKash / Nagad / Rocket, reports problems |
| Lender   | Accepts or rejects loan requests sent to them, reports problems |
| Monitor  | Approves or rejects new borrower accounts and helper-submitted registrations |
| Helper   | Registers borrowers who can't sign up themselves |
| Admin    | Creates lenders, monitors and helpers; edits or deletes users; views reported problems and the bank reserve |

## Getting started

**Requirements:** Visual Studio 2022 with the ".NET desktop development" workload, and
SQL Server (Express is fine).

1. Clone the repo:
   ```bash
   git clone https://github.com/atik806/Community-Microloan-platform.git
   ```
2. Restore the database: in SSMS, right-click **Databases → Import Data-tier Application**
   and pick `database/b_info.bacpac`. Keep the database name `b_info`.
3. Point the app at your server: edit the `b_info` connection string in
   `src/CommunityMicroloan/App.config`. The default is `.\SQLEXPRESS` with Windows
   authentication.
4. Open `Community_microloan_platform.sln` and press **F5**.

Loans accepted before the repayment fix have no outstanding balance recorded. To make
them repayable, run this once against `b_info`:

```sql
UPDATE LoanRequests SET due_amount = Ammount WHERE Status = 'Accepted' AND due_amount IS NULL;
```

**Demo logins** (hard-coded for development, see `Forms/Auth/login.cs`):
`admin@gmail.com` / `123` and `monitor@gmail.com` / `123`. Lenders, monitors and helpers
created by the admin log in with the email and password the admin set.

## Project structure

```
Community_microloan_platform.sln
database/
  b_info.bacpac              SQL Server database export (schema + sample data)
src/CommunityMicroloan/
  Program.cs                 Entry point, opens the Main (landing) form
  App.config                 Connection string lives here
  Data/Db.cs                 Reads the connection string for every form
  Forms/
    Public/                  Main, about, howitworks, process
    Auth/                    login, Form1 (borrower signup), forgetPassword, updatepassword
    Borrower/                b_dashboard, loanrequest, repay_loan, b_problem
    Lender/                  lender_dashboard, Lender_problem
    Monitor/                 monitor, Monitor_problem, Helper_requests
    Helper/                  hepler_dashboard, borrowerRegistration, h_repay
    Admin/                   admin, user creation, edit/list/update screens, bankreserve, problems
  Properties/                Assembly info, resources, settings
  Resources/                 Images used by the forms
```

## Known limitations

This is a learning project and is not ready to handle real money or personal data:

- Passwords are stored and compared in plain text.
- The admin and monitor demo accounts are hard-coded.
- "Forgot password" checks the email, NID and phone number on the account. There is no
  email or SMS code, so anyone who knows all three can still reset that password.
- Admin edit screens show users' passwords.
- `h_repay` (helper-assisted repayment) is a placeholder screen with no logic yet.

## License

For learning and educational purposes.
