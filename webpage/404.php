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
<link href="https://fonts.googleapis.com/css2?family=Luckiest+Guy&display=swap" rel="stylesheet">
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Rowdies:wght@300;400;700&display=swap" rel="stylesheet">
    <link rel="icon" type="image/x-icon" href="./kepek/vegleges1.4.png">
    
    <link rel="stylesheet" href="./css/navbar.css">
    
    <script src="./js/navbar.js"></script>
    <script src="./admin.js"></script>
    
    <link rel="stylesheet" href="./css/negyszaznegy.css">
    <title>404 Page Not Found</title>
    <style>

    </style>
</head>
<body>
  <div class="tartalom">    
    <nav class="sticky-navbar">
        <ul class="sidebar">
            <li onclick="hideSidebar()"><button onclick="NotFound()"><svg xmlns="http://www.w3.org/2000/svg"
                                height="26px" viewBox="0 -960 960 960" width="26px" fill="#FFFFFF">
                                <path
                                    d="m256-200-56-56 224-224-224-224 56-56 224 224 224-224 56 56-224 224 224 224-56 56-224-224-224 224Z" />
                            </svg></button></li>
           
                    <li><button onclick="Fooldal()"><b>Home</b></button></li>
                    <li><button onclick="Social()"><b>Social</b></button></li>
                    <li><button onclick="Challenges()"><b>Challenges</b></button></li>
                    <li><button onclick="Leaderboard()"><b>Leaderboard</b></button></li>
        </ul>
        <ul >
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
                    <a href="#"><svg xmlns="http://www.w3.org/2000/svg" height="26px" viewBox="0 -960 960 960" width="26px" fill="#FFFFFF"><path d="M120-240v-80h720v80H120Zm0-200v-80h720v80H120Zm0-200v-80h720v80H120Z"/></svg></a>
                </li>
        </ul>

        </nav>
        <main>
            <div class="kozepreigazit">
                <div class="luckiest-guy-regular">

                    <h1 class="tangletrack">404</h1>
                    <hr class="kozep">
                    <p>Oops, page not found!</p>  
                    <a href="./home"><img src="kepek/car-accident.png" alt="404 Error Image"  class="kozepreigazit-img" ></a>
                </div>
            </div>
        </main>
    <div class="download-bar">
           🚀 Ready to challenge your brain? 
<a href="https://www.test.tangletrack.com/TangleTrack.apk" download><span class="mobile-break">Download Tangle Track now!</span></a>
</div>


    <footer id="contact">
        <div class="footer-container">
            <div class="social-links">
                <a target="_blank" href="https://www.facebook.com/profile.php?id=61566264156754"><img src="./kepek/facebook_logo.avif" alt="Facebook"></a>
                <a target="_blank" href="https://www.x.com/Tangle_Track"><img src="./kepek/twitter_logo.avif" alt="Twitter"></a>
                <a target="_blank" href="https://www.youtube.com/@TangleTrack"><img src="./kepek/youtube_logo.png" alt="YouTube"></a>
                <a target="_blank" href="https://www.instagram.com/tangletrack/"><img src="./kepek/insta_logo.jpg" alt="Instagram"></a>
            </div>
            <div class="footer-buttons">
                <a href="#" target="_blank">Terms of Service</a>
                <a href="#" target="_blank">Privacy Policy</a>
                <a href="#" target="_blank">Help & Support</a>
            </div>
                <p>Hungary</p>
                <p>Tel: +36 70-531-1887</p>
                <p>Brainboost® © 2024 Devsisters Corp. All Rights Reserved.</p>
        </div>
    </footer>
</div>
</div>
</body>
</html>
