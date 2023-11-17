//function handleFocusAge(ref) {
//    // Get current date
//    var today = new Date();
//    var yyyy = today.getFullYear();
//    var mm = String(today.getMonth() + 1).padStart(2, '0');
//    var dd = String(today.getDate()).padStart(2, '0');
//    var maxDate = yyyy + '-' + mm + '-' + dd;
//    // Set maximum date for the date of birth input field
//    document.getElementById("inputDateOfBirth").setAttribute("max", maxDate);
//}

//// Function to handle focus out event for the age input field
//function handleFocusOutAge(ref) {
//    // Get current date
//    var today = new Date();
//    var yyyy = today.getFullYear();
//    var mm = String(today.getMonth() + 1).padStart(2, '0');
//    var dd = String(today.getDate()).padStart(2, '0');
//    var maxDate = yyyy + '-' + mm + '-' + dd;
//    // Set maximum date for the date of birth input field
//    document.getElementById("inputDateOfBirth").setAttribute("max", maxDate);

//    // Get the user's input date of birth
//    var dobInput = document.getElementById("inputDateOfBirth").value;

//    // Calculate the age based on the input date of birth
//    var dob = new Date(dobInput);
//    var age = today.getFullYear() - dob.getFullYear();

//    // Update the age input field with the calculated age
//    document.getElementById("age").value = age;

//    // Get error message element
//    var errorDateOfBirth = document.getElementById("errorDateOfBirth");

//    // Check if the age is less than 18
//    if (age < 18) {
//        ref.classList.add("invalid");
//        errorDateOfBirth.innerHTML = "Minimum age is 18";
//        document.getElementById("age").value = "";
//    } else if (dobInput == 0) {
//        // Check if date of birth is not entered
//        ref.classList.add("invalid");
//        errorDateOfBirth.innerHTML = "Please enter your date of birth";
//        document.getElementById("age").value = "";
//    } else {
//        // Clear validation errors
//        ref.classList.remove("invalid");
//        errorDateOfBirth.innerHTML = "";
//    }
//}
// Function to validate the first name input field

const stateCityMap = {
    "Andhra Pradesh": ["Hyderabad", "Visakhapatnam", "Vijayawada", "Guntur", "Nellore"],
    "Arunachal Pradesh": ["Itanagar", "Tawang", "Naharlagun", "Pasighat", "Roing"],
    "Assam": ["Guwahati", "Silchar", "Dibrugarh", "Jorhat", "Nagaon"],
    "Bihar": ["Patna", "Gaya", "Bhagalpur", "Muzaffarpur", "Purnia"],
    "Chhattisgarh": ["Raipur", "Bhilai", "Bilaspur", "Korba", "Durg"],
    "Goa": ["Panaji", "Vasco da Gama", "Margao", "Mapusa", "Ponda"],
    "Gujarat": ["Ahmedabad", "Surat", "Vadodara", "Rajkot", "Bhavnagar"],
    "Haryana": ["Chandigarh", "Faridabad", "Gurgaon", "Hisar", "Panipat"],
    "Himachal Pradesh": ["Shimla", "Mandi", "Solan", "Dharamshala", "Kullu"],
    "Jharkhand": ["Ranchi", "Jamshedpur", "Dhanbad", "Bokaro Steel City", "Hazaribagh"],
    "Karnataka": ["Bangalore", "Mysore", "Hubli-Dharwad", "Mangalore", "Belgaum"],
    "Kerala": ["Thiruvananthapuram", "Kochi", "Kozhikode", "Thrissur", "Kollam"],
    "Madhya Pradesh": ["Bhopal", "Indore", "Jabalpur", "Gwalior", "Ujjain"],
    "Maharashtra": ["Mumbai", "Pune", "Nagpur", "Thane", "Nashik"],
    "Manipur": ["Imphal", "Thoubal", "Bishnupur", "Kakching", "Churachandpur"],
    "Meghalaya": ["Shillong", "Tura", "Jowai", "Nongstoin", "Williamnagar"],
    "Mizoram": ["Aizawl", "Lunglei", "Saiha", "Champhai", "Serchhip"],
    "Nagaland": ["Kohima", "Dimapur", "Mokokchung", "Tuensang", "Wokha"],
    "Odisha": ["Bhubaneswar", "Cuttack", "Rourkela", "Berhampur", "Sambalpur"],
    "Punjab": ["Chandigarh", "Ludhiana", "Amritsar", "Jalandhar", "Patiala"],
    "Rajasthan": ["Jaipur", "Jodhpur", "Kota", "Bikaner", "Ajmer"],
    "Sikkim": ["Gangtok", "Namchi", "Mangan", "Singtam", "Gyalshing"],
    "Tamil Nadu": ["Chennai", "Coimbatore", "Madurai", "Tiruchirappalli", "Salem"],
    "Telangana": ["Hyderabad", "Warangal", "Nizamabad", "Karimnagar", "Ramagundam"],
    "Tripura": ["Agartala", "Udaipur", "Dharmanagar", "Ambassa", "Kailasahar"],
    "Uttar Pradesh": ["Lucknow", "Kanpur", "Agra", "Varanasi", "Meerut"],
    "Uttarakhand": ["Dehradun", "Haridwar", "Roorkee", "Haldwani", "Kashipur"],
    "West Bengal": ["Kolkata", "Howrah", "Asansol", "Siliguri", "Durgapur"]
};

function populateStates() {
    const stateSelect = document.getElementById("inputState");

    // Clear existing state options
    stateSelect.innerHTML = '<option value="" disabled selected>Select a state</option>';

    // Populate the state dropdown with states from the state-city mapping
    Object.keys(stateCityMap).forEach(state => {
        const option = document.createElement("option");
        option.value = state;
        option.textContent = state;
        stateSelect.appendChild(option);
    });

    // Trigger the city population when a state is selected
    stateSelect.addEventListener("change", updateCityOptions);
}

function updateCityOptions() {
    const stateSelect = document.getElementById("inputState");
    const citySelect = document.getElementById("inputCity");

    // Clear existing city options
    citySelect.innerHTML = '<option value="" disabled selected>Select a city</option>';

    const selectedState = stateSelect.value;
    const citiesInSelectedState = stateCityMap[selectedState] || [];

    // Populate the city dropdown with cities from the selected state
    citiesInSelectedState.forEach(city => {
        const option = document.createElement("option");
        option.value = city;
        option.textContent = city;
        citySelect.appendChild(option);
    });
}

// Call the populateStates function when the window is loaded
window.onload = populateStates;




document.addEventListener("DOMContentLoaded", function () {
    // Function to handle focus out event for the date of birth input field
    function handleFocusOutAge() {
        // Get the user's input date of birth
        var dobInput = document.getElementById("inputDateOfBirth").value;

        // Check if the date of birth is not empty
        if (dobInput) {
            // Calculate the age based on the input date of birth
            var dob = new Date(dobInput);
            var today = new Date();
            var age = today.getFullYear() - dob.getFullYear();

            // Get error message element
            var errorDateOfBirth = document.getElementById("errorDateOfBirth");

            // Check if age is not greater than or equal to 18 and display an error message
            if (age < 18) {
                errorDateOfBirth.innerHTML = "Age must be 18 and above.";
            } else {
                // Clear any previous error messages
                errorDateOfBirth.innerHTML = "";
            }

            // Disable future dates
            var maxDate = new Date().toISOString().split('T')[0];
            document.getElementById("inputDateOfBirth").setAttribute("max", maxDate);
        } else {
            // Handle the case where the date of birth is not provided
            var errorDateOfBirth = document.getElementById("errorDateOfBirth");
            errorDateOfBirth.innerHTML = "Please enter your date of birth.";
        }
    }

    // Ensure that the handleFocusOutAge function is called when the input loses focus
    var inputDateOfBirth = document.getElementById("inputDateOfBirth");
    inputDateOfBirth.addEventListener("blur", handleFocusOutAge);
});


function validateFirstName(ref) {
    // Regular expression to check for special characters
    const hasSpecialChar = /[^A-Za-z]/; // Only letters allowed

    // Check if the first name is less than 3 characters or contains special characters
    if (ref.value.length < 3 || hasSpecialChar.test(ref.value)) {
        ref.classList.add("invalid");
        errorFirstName.innerHTML = "First name should be a minimum of 3 letters and should contain only alphabetic characters";
    } else {
        ref.classList.remove("invalid");
        errorFirstName.innerHTML = "";
    }

    // Check if the first name is empty
    if (ref.value.trim() === '') {
        displayError('errorFirstName', 'First name is required.');
        // Assuming you have a variable named isValid declared in the scope
        isValid = false;
    } else {
        isValid = true; // Set to true if the field is not empty
    }

    // Event handler for onFocus event to clear validation error on focus
    ref.onfocus = function () {
        if (this.classList.contains("invalid")) {
            this.classList.remove("invalid");
            errorFirstName.innerHTML = "";
        }
    };
}


// Function to validate the last name input field
function validateLastName(ref) {
    // Regular expression to check for special characters
    const hasSpecialChar = /[^A-Za-z]/; // Only letters allowed

    // Check if the last name is less than 3 characters or contains special characters
    if (ref.value.length < 2 || hasSpecialChar.test(ref.value)) {
        ref.classList.add("invalid");
        errorLastName.innerHTML = "Last name should be minimum of 3 letters and should contain only alphabetic characters";
    } else {
        ref.classList.remove("invalid");
        errorLastName.innerHTML = "";
    }

    // Event handler for onFocus event to clear validation error on focus
    ref.onfocus = function () {
        if (this.classList.contains("invalid")) {
            this.classList.remove("invalid");
            errorLastName.innerHTML = "";
        }
    };
}
document.getElementById("inputnumber").addEventListener("focusout", function () {
    validateNumber(this);
});

// Function to validate a phone number
function validateNumber(ref) {
    // Check if the length of the number is not 10 digits
    if (ref.value.length !== 10) {
        ref.classList.add("invalid");
        errorNumber.innerHTML = "Enter a valid number of 10 digits";
    } else {
        ref.classList.remove("invalid");
        errorNumber.innerHTML = "";
    }

    // Event handler for onFocus event to clear validation error on focus
    ref.onfocus = function () {
        if (this.classList.contains("invalid")) {
            this.classList.remove("invalid");
            errorNumber.innerHTML = "";
        }
    };
}
// Function to validate the address input field
function validateAddress(ref) {
    let error = document.getElementById("errorAddress");

    // Check if the address is empty
    if (ref.value.trim() === "") {
        ref.classList.add("invalid");
        error.innerHTML = "Address cannot be empty";
    }

    // Event handler for onFocus event to clear validation error on focus
    ref.onfocus = function () {
        if (this.classList.contains("invalid")) {
            this.classList.remove("invalid");
            error.innerHTML = "";
        }
    };
}
// Event listener for the password input field on focus out event
// Function to validate the password input field
//function validatePassword(ref) {
//    let errorPassword = document.getElementById("errorPassword");

//    // Updated password pattern
//    const passwordPattern = /^(?=.*[A-Z])(?=.*[!@#$%^&*])(?=.*[0-9]).{8,}$/;

//    // Check if the password meets the criteria
//    if (!passwordPattern.test(ref.value)) {
//        ref.classList.add("invalid");
//        errorPassword.innerHTML = "Password must contain at least 1 uppercase letter, 1 special character, 1 number, and be at least 8 characters long";
//    } else {
//        ref.classList.remove("invalid");
//        errorPassword.innerHTML = "";
//    }
//}
// Event listener for the password input field on focus out event

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
// Function to validate the email input field
function validateEmail(ref) {
    let error = document.getElementById("errorMail");
    const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    const email = ref.value.toLowerCase(); // Convert email to lowercase for case-insensitive comparison

    // Check if the email is valid
    if (!emailPattern.test(email)) {
        ref.classList.add("invalid");
        error.innerHTML = "Enter a valid email address";
        document.getElementById("Username").value = "";
    } else {
        ref.classList.remove("invalid");
        error.innerHTML = "";
        // Set the entered email as the username
        document.getElementById("Username").value = email;
    }

    // Event handler for onFocus event to clear validation error on focus
    ref.onfocus = function () {
        if (this.classList.contains("invalid")) {
            this.classList.remove("invalid");
            error.innerHTML = "";
        }
    };
}



function ValidateLoginUsername(ref) {
    let errorLoginUsername = document.getElementById("errorLoginUsername");

    // Check if the username is empty
    if (ref.value.trim() === "") {
        ref.classList.add("invalid");
        errorLoginUsername.innerHTML = "Please enter your user name/email id";
    }

    // Event handler for onFocus event to clear validation error on focus
    ref.onfocus = function () {
        if (this.classList.contains("invalid")) {
            this.classList.remove("invalid");
            errorLoginUsername.innerHTML = "";
        }
    };
}

// Function to validate login password input field
function ValidateLoginPassword(ref) {
    let errorLoginPassword = document.getElementById("errorLoginPassword");

    // Check if the password is empty
    if (ref.value.trim() === "") {
        ref.classList.add("invalid");
        errorLoginPassword.innerHTML = "Please enter password";
    }

    // Event handler for onFocus event to clear validation error on focus
    ref.onfocus = function () {
        if (this.classList.contains("invalid")) {
            this.classList.remove("invalid");
            errorLoginPassword.innerHTML = "";
        }
    };
}
function togglePasswordVisibility() {
    var passwordInput = document.getElementById('password-input');
    var eyeIcon = document.getElementById('eye-icon');

    // Toggle the type attribute between 'password' and 'text'
    if (passwordInput.type === 'password') {
        passwordInput.type = 'text';
        eyeIcon.textContent = '👁️'; // Change to open eye
    } else {
        passwordInput.type = 'password';
        eyeIcon.textContent = '👁️'; // Change to closed eye
    }
}




// Function to validate the form before submission
function validateForm() {
    var requiredFields = ['inputfName', 'lastName', 'inputDateOfBirth', 'inputnumber', 'emailInput', 'inputAddress', 'inputState', 'inputCity', 'password', 'confirmPassword'];
    var isValid = true;

    for (var i = 0; i < requiredFields.length; i++) {
        var fieldName = requiredFields[i];
        var field = document.getElementById(fieldName);
        var errorElement = document.getElementById('error' + fieldName.charAt(0).toUpperCase() + fieldName.slice(1));

        if (field.value.trim() === '') {
            isValid = false;
            displayErrorMessage(fieldName, 'This field is required.');
        } else {
            clearErrorMessage(fieldName);
        }
    }

    return isValid;
}

function displayErrorMessage(fieldId, errorMessage) {
    var errorElement = document.getElementById('error' + fieldId.charAt(0).toUpperCase() + fieldId.slice(1));

    if (errorElement) {
        errorElement.innerHTML = errorMessage;
        document.getElementById(fieldId).classList.add("invalid");
    }
}

function clearErrorMessage(fieldId) {
    var errorElement = document.getElementById('error' + fieldId.charAt(0).toUpperCase() + fieldId.slice(1));

    if (errorElement) {
        errorElement.innerHTML = '';
        document.getElementById(fieldId).classList.remove("invalid");
    }
}

document.addEventListener("DOMContentLoaded", function () {
    var form = document.getElementById("main-content");
    form.addEventListener("submit", function (event) {
        if (!validateForm()) {
            event.preventDefault(); // Prevent form submission if validation fails
        }
    });
});
