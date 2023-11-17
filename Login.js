function validateUsername(input) {
    // Get the input value
    var value = input.value.trim();

    // Check if the value is empty
    if (value === "") {
        document.getElementById('errorUsername').innerText = 'Enter your valid username';
        return false;
    }

    // Clear any previous error message
    document.getElementById('errorUsername').innerText = '';

    return true;
}

// Add an event listener to call the validation function on blur
document.getElementById('inputUsername').addEventListener('blur', function () {
    validateUsername(this);
});

function validateOldPassword(input) {
    // Get the input value
    var value = input.value.trim();

    // Check if the value is empty
    if (value === "") {
        document.getElementById('errorOldPassword').innerText = 'Enter your password';
        return false;
    }

    // Clear any previous error message
    document.getElementById('errorOldPassword').innerText = '';

    return true;
}
