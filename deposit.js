$(document).ready(function () {
    // Function to perform form validation
    function validateForm() {
        var isValid = true;

        // Check UserId
        var userId = $('#UserId').val().trim();
        if (userId === '') {
            alert('Please enter a UserId.');
            isValid = false;
        }

        // Check Amount
        var amount = $('#Amount').val().trim();
        if (amount === '') {
            alert('Please enter an Amount.');
            isValid = false;
        }

        // Check AccountNumber
        var accountNumber = $('#AccountNumber').val().trim();
        if (accountNumber === '') {
            alert('Please enter an Account Number.');
            isValid = false;
        }

        // Check IFSCCode
        var ifscCode = $('#IFSCCode').val().trim();
        if (ifscCode === '') {
            alert('Please enter an IFSC Code.');
            isValid = false;
        }

        return isValid;
    }

    // Attach form submission event
    $('#depositForm').submit(function (e) {
        // Perform validation before submitting the form
        if (!validateForm()) {
            e.preventDefault(); // Prevent form submission if validation fails
        }
    });
});