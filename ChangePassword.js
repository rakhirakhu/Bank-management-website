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
        document.getElementById('errorOldPassword').innerText = 'Enter old password.';
        return false;
    }

    // Clear any previous error message
    document.getElementById('errorOldPassword').innerText = '';

    return true;
}

////// Add an event listener to call the validation function on blur
////document.getElementById('inputOldPassword').addEventListener('blur', function () {
////    validateOldPassword(this);
////});
////function validateNewPassword(input) {
////    // Get the input value
////    var value = input.value.trim();

////    // Check if the value is empty
////    if (value === "") {
////        document.getElementById('errorNewPassword').innerText = 'Enter new password.';
////        return false;
////    }

////    // Clear any previous error message
////    document.getElementById('errorNewPassword').innerText = '';

////    return true;
////}

////// Add an event listener to call the validation function on blur
////document.getElementById('inputNewPassword').addEventListener('blur', function () {
////    validateNewPassword(this);
////});

document.getElementById("password").addEventListener("focusout", function () {
    validatePassword(this);
});

function validatePassword(ref) {
    let errorPassword = document.getElementById("errorPassword");
    const passwordPattern = /^(?=.*[A-Z])(?=.*[!@#$%^&*])(?=.*[0-9]).{8,}$/;

    if (!passwordPattern.test(ref.value)) {
        ref.classList.add("invalid");
        errorPassword.innerHTML = "Password must contain at least 1 uppercase letter, 1 special character, 1 number, and be at least 8 characters long";
    } else {
        ref.classList.remove("invalid");
        errorPassword.innerHTML = "";
    }
}



// Function to validate the confirm password input field
function validateConfirmPassword(ref) {
    let password = document.getElementById("password").value;
    let error = document.getElementById("errorConfirmPassword");

    // Check if the confirm password matches the password
    if (ref.value !== password) {
        ref.classList.add("invalid");
        errorConfirmPassword.innerHTML = "Passwords do not match.";
    } else {
        ref.classList.remove("invalid");
        errorConfirmPassword.innerHTML = "";
    }
}

// Function to toggle password visibility
function togglePasswordVisibility() {
    var passwordInput = document.getElementById("password");
    var icon = document.querySelector(".password-toggle");

    // Toggle password visibility
    if (passwordInput.type === "password") {
        passwordInput.type = "text";
        icon.classList.remove("fa-eye-slash");
        icon.classList.add("fa-eye");
    } else {
        passwordInput.type = "password";
        icon.classList.remove("fa-eye");
        icon.classList.add("fa-eye-slash");
    }
}