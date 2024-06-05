function autoDelete() {
    var msg = document.getElementById('alertOnDelete').value;
    if (msg == 'CurrentUserDeleted') {
        return alert("O usuário desta sessão não existe mais, você está sendo desconectado.");
    }
}

function userCreated() {
    var msg = document.getElementById('alertOnCreate').value;
    if (msg == 'NewUserSuccess') {
        return alert("Usuário Cadastrado com sucesso, faça login para continuar.");
    }
}

function loginTrial() {
    var msg = document.getElementById('alertOnFailedLogin').value;
    if (msg == 'LoginFailed') {
        return alert("Usuário ou senha incorretos.");
    }
}