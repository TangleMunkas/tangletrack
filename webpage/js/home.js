  
function SeeMore() {
    window.open('https://www.youtube.com/@TangleTrack', '_blank'); 
    
}

function AppStore() {
    window.open('https://apps.apple.com/', '_blank');
}

function GooglePlay() {
    window.open('https://play.google.com/', '_blank');
}



let currentIndex = 0;

function showSlide(index) {
    const slides = document.querySelectorAll('.slide');
    const dots = document.querySelectorAll('.dot');
    const tabs = document.querySelectorAll('.tab');
    const track = document.querySelector('.carousel-track');

    const totalSlides = slides.length;
    
    if (index >= totalSlides) {
        currentIndex = 0;
    } else if (index < 0) {
        currentIndex = totalSlides - 1;
    } else {
        currentIndex = index;
    }

    const offset = -currentIndex * 100 + "%";
    track.style.transform = `translateX(${offset})`;

    // Minden dot visszaállítása notactive.png-re
    dots.forEach(dot => dot.src = "./kepek/notactive.png");

    // Az aktuális dot képe active.png lesz
    if (dots[currentIndex]) {
        dots[currentIndex].src = "./kepek/active.png";
    }

    // Tabok állapotának frissítése
    tabs.forEach(tab => tab.classList.remove('active'));
    if (tabs[currentIndex]) {
        tabs[currentIndex].classList.add('active');
    }
}

function prevSlide() {
    showSlide(currentIndex - 1);
}

function nextSlide() {
    showSlide(currentIndex + 1);
}

function goToSlide(index) {
    showSlide(index);
}

// Automatikus csúsztatás
setInterval(() => {
    nextSlide();
}, 5000);

// Első megjelenítés
showSlide(currentIndex);
