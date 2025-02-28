function Fooldal() {
    window.location.href = './home'
}
function Social(){
    window.location.href = './social'
}

function Events(){
    window.location.href = './event'
}

function Leaderboard(){
    window.location.href = './leaderboard'
}
function Shop(){
    window.location.href = './shop'
}
function NotFound(){
    window.location.href = './404'
}
function toggleSidebar() {
    const sidebar = document.querySelector('.sidebar');
    sidebar.classList.toggle('active');
}
function showSidebar(){
    const sidebar = document.querySelector('.sidebar')
     sidebar.classList.remove('active');
}
function hideSidebar() {
    const sidebar = document.querySelector('.sidebar');
    sidebar.classList.remove('active');
}

/*
    // Új lap            window.open('https://www.youtube.com/@TangleTrack', '_blank');

    // Ugyanazon lap     window.location.href = 'https://www.youtube.com/@TangleTrack';

 Hosszú érintés tiltása mobilon*/
  document.addEventListener('touchstart', function(e) {
    if (e.target.tagName === 'IMG') {
      let timeout = setTimeout(() => {
        e.preventDefault();
      }, 500); // Hosszú érintés időtartama
      e.target.addEventListener('touchend', () => clearTimeout(timeout), { once: true });
    }
  });

  // Jobb kattintás tiltása képeken
  document.addEventListener('contextmenu', function(e) {
    if (e.target.tagName === 'IMG') {
      e.preventDefault();
    }
  });
  
  document.addEventListener('contextmenu', function (e) {
    if (e.target.closest('.no-download')) {
        e.preventDefault(); // Jobb kattintás tiltása
    }
});

document.addEventListener('touchstart', function (e) {
    if (e.target.closest('.no-download')) {
        let timeout = setTimeout(() => e.preventDefault(), 500); // Hosszú érintés tiltása
        e.target.addEventListener('touchend', () => clearTimeout(timeout), { once: true });
    }
});
  
  
  document.addEventListener("DOMContentLoaded", function () {
  // Kiválasztjuk az összes linket és gombot a no-highlight osztályon belül
  const elements = document.querySelectorAll(".no-highlight a, .no-highlight button");

  elements.forEach((element) => {
    // Letiltjuk a kék érintési visszajelzést
    element.style.webkitTapHighlightColor = "transparent";

    // Letiltjuk az outline-t fókusz esetén
    element.addEventListener("focus", function () {
      element.style.outline = "none";
      element.style.boxShadow = "none";
    });

    // Biztonság kedvéért jobb kattintásra is reagálunk
    element.addEventListener("contextmenu", function (e) {
      e.preventDefault(); // Megakadályozza a jobb kattintás menüt
    });
  });
});

  document.addEventListener("DOMContentLoaded", function () {
  // Kiválasztjuk az összes listaelemet a .no-highlight osztályon belül
  const listItems = document.querySelectorAll(".no-highlight li");

  listItems.forEach((li) => {
    // Letiltjuk a kék érintési visszajelzést
    li.style.webkitTapHighlightColor = "transparent";

    // Letiltjuk az outline-t fókusz esetén
    li.addEventListener("focus", function () {
      li.style.outline = "none";
      li.style.boxShadow = "none";
    });

    // Biztonság kedvéért jobb kattintásra is reagálunk
    li.addEventListener("contextmenu", function (e) {
      e.preventDefault(); // Megakadályozza a jobb kattintás menüt
    });
  });
});



