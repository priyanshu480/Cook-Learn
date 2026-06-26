@model dotnetapp.Models.RegisterViewModel

<h2>Register</h2>

<form method="post" action="/Account/Register">

    <input type="email" name="Email" />
    <input type="password" name="Password" />
    <input type="password" name="ConfirmPassword" />

    <button type="submit">Register</button>

</form>

<a href="/Account/Login">Login</a>



@model dotnetapp.Models.LoginViewModel@model dotnetapp.Models.Login2>Login</h2>

<form method="post" action="/Account/Login">

    <input type="email" name="Email" />
    <input type="password" name="Password" />

    <button type="submit">Login</button>

</form>

<a href="/Account/Register">Register</a>


