<?php
// Adatbázis kapcsolat
$host = 'localhost';
$dbname = 'rh41261_tangle';
$username = 'rh41261_teszt';
$password = '!TangleTrack69!';

try {
    $pdo = new PDO("mysql:host=$host;dbname=$dbname;charset=utf8", $username, $password);
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
} catch (PDOException $e) {
    die("Adatbázis hiba: " . $e->getMessage());
}

// Látogató adatok lekérése
$ip = $_SERVER['REMOTE_ADDR'];
$user_agent = $_SERVER['HTTP_USER_AGENT'];
$browser = "Ismeretlen";
$device = "Ismeretlen";
$page = $_SERVER['REQUEST_URI']; // Az éppen megnyitott oldal

// Böngésző detektálás
if (strpos($user_agent, 'Chrome') !== false) {
    $browser = "Google Chrome";
} elseif (strpos($user_agent, 'Firefox') !== false) {
    $browser = "Mozilla Firefox";
} elseif (strpos($user_agent, 'Safari') !== false) {
    $browser = "Safari";
} elseif (strpos($user_agent, 'Edge') !== false) {
    $browser = "Microsoft Edge";
} elseif (strpos($user_agent, 'MSIE') !== false || strpos($user_agent, 'Trident') !== false) {
    $browser = "Internet Explorer";
}

// Eszköz detektálás
if (preg_match('/mobile/i', $user_agent)) {
    $device = "Mobil";
} elseif (preg_match('/tablet/i', $user_agent)) {
    $device = "Tablet";
} else {
    $device = "PC";
}

// Ország meghatározása (IP API segítségével)
$country = "Ismeretlen";
$ip_api_url = "http://ip-api.com/json/$ip?fields=country";
$response = @file_get_contents($ip_api_url);
if ($response) {
    $data = json_decode($response, true);
    if (!empty($data['country'])) {
        $country = $data['country'];
    }
}

// Adatok mentése az adatbázisba
$sql = "INSERT INTO visitors (ip, country, browser, device, page) VALUES (?, ?, ?, ?, ?)";
$stmt = $pdo->prepare($sql);
$stmt->execute([$ip, $country, $browser, $device, $page]);
?>
<!DOCTYPE html>
<html lang="hu">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="preconnect" href="https://fonts.googleapis.com">
<link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
<link href="https://fonts.googleapis.com/css2?family=Rowdies:wght@300;400;700&display=swap" rel="stylesheet">
    
    <link rel="stylesheet" href="./css/navbar.css">
    <link rel="stylesheet" href="./css/social.css">
  
    <script src="./js/navbar.js"></script>
    <script src="./js/social.js"></script>

    
    <link rel="icon" type="image/x-icon" href="./kepek/vegleges1.4.png">
    <title>Tangle Social</title>
    <style>
.see-more {
            -webkit-tap-highlight-color: transparent !important;
}
.no-highlight .no-download {
    height: auto !important;
    overflow: visible !important;
}

    #responseMessage {
        font-family: 'Arial', sans-serif; /* Betűtípus */
    }
      </style>
</head>
<body>
    <div class="no-highlight">
        <div class="no-download">
    
    <nav class="sticky-navbar">
                <ul class="sidebar">
                    <li onclick="hideSidebar()"><button><svg xmlns="http://www.w3.org/2000/svg"
                                height="26px" viewBox="0 -960 960 960" width="26px" fill="#FFFFFF">
                                <path
                                    d="m256-200-56-56 224-224-224-224 56-56 224 224 224-224 56 56-224 224 224 224-56 56-224-224-224 224Z" />
                            </svg></button></li>

                    <li><button onclick="Fooldal()"><b>Home</b></button></li>
                    <li><button onclick="Social()"><b>Social</b></button></li>
                    <li><button onclick="Challenges()"><b>Challenges</b></button></li>
                    <li><button onclick="Leaderboard()"><b>Leaderboard</b></button></li>
                </ul>
                <ul>
                    <li class="nav-item"><button onclick="Fooldal()"><img src="./kepek/hatternelkul.png" alt=""
                                width="90" height="auto" draggable="false" onclick="Fooldal()"></button>

                    </li>
                    
                                        <li class="hideOnMobile nav-item"><button onclick="Fooldal()"><b>Home</b></button>
                    </li>
                    <li class="hideOnMobile nav-item"><button onclick="Social()"><b>Social</b></button>
                    </li>
                    <li class="hideOnMobile nav-item"><button onclick="Challenges()"><b>Challenges</b></button>

                    </li>
                    <li class="hideOnMobile nav-item"><button onclick="Leaderboard()"><b>Leaderboard</b></button>

                    </li>
                    <li class="menu-button" onclick="toggleSidebar()">
                        <button>
                            <svg xmlns="http://www.w3.org/2000/svg" height="26px" viewBox="0 -960 960 960" width="26px"
                                fill="#FFFFFF">
                                <path d="M120-240v-80h720v80H120Zm0-200v-80h720v80H120Zm0-200v-80h720v80H120Z" />
                            </svg>
                        </button>
                </ul>
            </nav>
    </div>
    </div>
    <header>
        <div class="kozepreigazit">
            <div class="logo">
                <img src="./kepek/tangletracklogo.png" alt="Game Logo" draggable="false">
            </div>
            <p style="color:black;"><b></b></p>
            
        </div>
    </header>
    <div class="download-bar">
           🚀 Ready to challenge your brain? 
<a href="https://www.tangletrack.com/TangleTrack.apk" download><span class="mobile-break">Download Tangle Track now!</span></a>
</div>
    <div class="no-highlight">
        <div class="no-download">
    <section id="social">
            <div class="social-section">
          
          <span class="tangle"><h1 class="tangletrack">Tangle Track</h1></span>
            <hr class="kozep">
            <h1 class="track">Social</h1>
            <br>
          <div class="social-links-bigger">
    <img src="./kepek/youtube_logo.png" alt="YouTube" onclick="window.open('https://www.youtube.com/@TangleTrack', '_blank')">
    <img src="./kepek/insta_logo.jpg" alt="Instagram" onclick="window.open('https://www.instagram.com/tangletrack/', '_blank')">
    <img src="./kepek/twitter_logo.avif" alt="Twitter" onclick="window.open('https://www.x.com/Tangle_Track', '_blank')">
    <img src="./kepek/tiktok_logo.png" alt="TikTok" onclick="window.open('https://www.tiktok.com/@tangletrack?_t=ZN-8tKZPxckkCc&_r=1', '_blank')">
    <img src="./kepek/facebook_logo.avif" alt="Facebook" onclick="window.open('https://www.facebook.com/profile.php?id=61566264156754', '_blank')">
    <img src="./kepek/discord_logo.jpg" alt="Discord" onclick="window.open('https://discord.gg/nQeeXuER', '_blank')">
</div>  
          </div>
          <br>
          <p>Follow the official channels to get the latest Tangle Track news!</p>
        
      </section>
      <section id="contactus">
          <div class="contactus-section">
        <div class="contactus-container">
            <span class="tangle"><h1 class="tangletrack">Tangle Track</h1></span>
            <hr class="kozep">
            <h1 stlye="color:white;" class="track">Contact us</h1>
            <form id="contactForm">
    <div class="form-group">
        <label for="name">Name <span>*</span></label>
        <input type="text" id="name" name="name" placeholder="Enter your name" required>
    </div>
    <div class="form-group">
        <label for="email">E-Mail <span>*</span></label>
        <input type="email" id="email" name="email" placeholder="Enter your email" required>
    </div>
    <div class="form-group">
        <label for="message">Message <span>*</span></label>
        <textarea id="message" name="message" rows="5" required></textarea>
    </div>
    <button type="submit" class="contactus-button">Submit</button>
    <div id="responseMessage"></div> <!-- Ide kerül az üzenet -->
</form>

<script>
document.getElementById("contactForm").addEventListener("submit", function(event) {
    event.preventDefault(); // Megakadályozza az oldal újratöltését

    let formData = new FormData(this); // Az űrlap adatainak begyűjtése

    fetch("send_email.php", { // AJAX kérés a PHP fájlnak
        method: "POST",
        body: formData
    })
    .then(response => response.text()) // A PHP válaszának feldolgozása
    .then(data => {
        let responseMessage = document.getElementById("responseMessage");

        if (data.includes("success")) {
            responseMessage.innerHTML = "Üzenet sikeresen elküldve!";
            responseMessage.style.backgroundColor = "rgba(0, 255, 0, 0.2)";
            responseMessage.style.color = "white";
            document.getElementById("contactForm").reset(); // Mezők törlése
        } else {
            responseMessage.innerHTML = "Üzenet küldése sikertelen!<br>Ellenőrizze üzenetét";
            responseMessage.style.backgroundColor = "rgba(255, 0, 0, 0.2)";
            responseMessage.style.color = "white";
        }

        responseMessage.style.display = "block"; // Üzenet megjelenítése
        responseMessage.style.padding = "10px";
        responseMessage.style.marginTop = "10px";
        responseMessage.style.borderRadius = "5px";
    })
    .catch(error => {
        let responseMessage = document.getElementById("responseMessage");
        responseMessage.innerHTML = "Hiba történt! Próbálja újra később.";
        responseMessage.style.backgroundColor = "rgba(255, 0, 0, 0.2)";
        responseMessage.style.color = "white";
        responseMessage.style.display = "block";
        responseMessage.style.padding = "10px";
        responseMessage.style.marginTop = "10px";
        responseMessage.style.borderRadius = "5px";
    });
});
</script>


        </div>
        </div>
    </section>
       <div class="download-bar">
           🚀 Ready to challenge your brain? 
<a href="https://www.tangletrack.com/TangleTrack.apk" download><span class="mobile-break">Download Tangle Track now!</span></a>
</div>
    <footer id="contact">
        <div class="footer-container">
           
            <div class="footer-buttons">
                <a href="#" target="_blank">Terms of Service</a>
                <a href="#" target="_blank">Privacy Policy</a>
                <a href="#" target="_blank">Help & Support</a>
            </div>
                <p>Hungary</p>
                <p></p>
                <p>Brainboost® © 2024. All Rights Reserved.</p>
        </div>
    </footer>
</div>
</div>
</body>
</html>
