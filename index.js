document.addEventListener('DOMContentLoaded', function () {
    // JavaScript code for Image Slider
    let currentIndex = 0;
    const slides = document.querySelectorAll('.image-slider img');
    const totalSlides = slides.length;

    function showSlide(index) {
        slides.forEach((slide) => (slide.style.display = 'none'));
        slides[index].style.display = 'block';
    }

    function nextSlide() {
        currentIndex = (currentIndex + 1) % totalSlides;
        showSlide(currentIndex);
    }

    function prevSlide() {
        currentIndex = (currentIndex - 1 + totalSlides) % totalSlides;
        showSlide(currentIndex);
    }

    // Auto slide change every 3 seconds (adjust as needed)
    setInterval(nextSlide, 3000);

    // Add event listeners for next and previous buttons if needed
    // Example: document.getElementById('nextButton').addEventListener('click', nextSlide);
    // Example: document.getElementById('prevButton').addEventListener('click', prevSlide);
});