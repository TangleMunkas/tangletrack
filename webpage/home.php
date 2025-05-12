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
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Rowdies:wght@300;400;700&display=swap" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css2?family=Luckiest+Guy&display=swap" rel="stylesheet">
    
    <meta name="description" content="Tangle Track is a challenging traffic puzzle game where players must untangle gridlocked cars to solve intricate levels. Enjoy multiple game modes including campaign, online 1v1 matches, and a level editor. Free to play on Android with rewards for optional ads and in-app purchases. Compete on global and local leaderboards for the top spot!">
    <meta name="keywords" content="Tangle Track, Rush Hour game, Traffic Jam, Traffic puzzle, mobile game, Logic game, Challenging puzzles, Strategy game, Relaxing yet tricky game, Family-friendly puzzle game, Android, Tangle Trace, Trangle Track, Tangletrack">
    <meta name="robots" content="index, follow">
    
    <link rel="icon" type="image/x-icon" href="./kepek/vegleges1.4.png">
    <title>Tangle Track</title>
    
    <link rel="stylesheet" href="./css/navbar.css">
    <link rel="stylesheet" href="./css/telcss.css">
    <link rel="stylesheet" href="vau.css">
    
    <script src="./js/navbar.js"></script>
    <script src="./js/home.js"></script>
    
    <style>
        .see-more {
            -webkit-tap-highlight-color: transparent !important;

        }

        .no-highlight .no-download {
            height: auto !important;
            overflow: visible !important;
        }
         .download-bar {
    background:linear-gradient(135deg, #ff6f00, #ffcc00, #d1d1d1);
    color: white;
    padding: 1rem;
    text-align: center;
    font-size: 1rem;
    font-weight: 500;
    position: relative;
  }
  .download-bar a {
    color: #fff;
    font-weight: bold;
    margin-left: 0.5rem;
    text-decoration: none;
  }
  .download-bar a:hover {
    text-decoration: none;
  }

        

        .download-btn {
            
            border: none;
            padding: 1rem 2rem;
            border-radius: 50px;
            font-size: 1.5rem;
            font-weight: bold;
            color: black;
            cursor: pointer;
            transition: 0.3s ease;
            text-decoration: none;
            display: inline-block;
            margin-top: 1.5rem;
            background: linear-gradient(135deg, #ff6f00, #ffcc00, #d1d1d1);
            background-size: 200%;
            background-position: left;
            transition: background-position 0.6s ease-in-out;
        }

        .download-btn:hover {
            background-color: rgba(130, 130, 130, 0.1);
            transform: scale(1.05);
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.2);
            background-position: right;
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
                    </li>
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
            <div class="download-buttons kozepregazit">
                <a href="https://www.tangletrack.com/TangleTrack.apk" download>
                    <img src="./kepek/button_download_apk.png" alt="Android APK" draggable="false">
                </a>
                <a>
                    <img src="./kepek/button_download_soon.png" alt="Google Play" draggable="false">
                </a>
            </div>

        </div>

    </header>
    <div class="no-highlight">
        <div class="no-download">
            <div class="download-bar">
           🚀 Ready to challenge your brain? 
<a href="https://www.tangletrack.com/TangleTrack.apk" download><span class="mobile-break">Download Tangle Track now!</span></a>
</div>

            <section id="news">
                <div class="news-section">
                    <h1 class="tangletrack"><span class="tangle">Tangle Track</span></h1>
                    <hr class="kozep">
                    <h1 class="track">NEWS</h1>
                    <br>
                    <div class="news-grid">
                        <div class="news-item">
                            <img src="./kepek/image_ingame.png" alt="Logical Adventures" draggable="false">
                            <p>Logical Adventures</p>
                        </div>
                        <div class="news-item">
                            <img src="./kepek/image_levels.png" alt="Campaign Mode" draggable="false">
                            <p>Campaign Mode</p>
                        </div>
                        <div class="news-item">
                            <img src="./kepek/image_magic.png" alt="Helpers And Obstructionists" draggable="false">
                            <p>Helpers And Obstructionists</p>
                        </div>
                        <div class="news-item">
                            <img src="./kepek/task.png" alt="Watch And Learn" draggable="false">
                            <p>Watch And Learn</p>
                        </div>
                        <div class="news-item">
                            <img src="./kepek/image_customization.png" alt="Customize Your Character" draggable="false">
                            <p>Customize Your Character</p>
                        </div>
                        <div class="news-item">
                            <img src="./kepek/image_build.png" alt="Build Own Levels" draggable="false">
                            <p>Build Own Levels</p>
                        </div>
                    </div>
                    <br>
                    <div class="see-more">
                        <a class="download-btn" href="SeeMore()"
                    target="_blank">
                    See More
                </a>
                    </div>
                    

                </div>
            </section>
            
            
            
                
            
            



            <section id="howtoplay">
                <div class="howtoplay-section">
                    <h1 class="tangletrack"><span class="tangle">Tangle Track</span></h1>
                    <hr class="kozep">
                    <h1 class="track">HOW TO PLAY</h1>

                </div>
            </section>
            <div class="download-bar">
           🚀 Ready to challenge your brain? 
<a href="https://www.tangletrack.com/TangleTrack.apk" download><span class="mobile-break">Download Tangle Track now!</span></a>
</div>
            <section id="aboutgame">
                <div class="aboutgame-section">
                    <h1 class="tangletrack"><span class="tangle">Tangle Track</span></h1>
                    <hr class="kozep">
                    <h1 class="track">ABOUT THE GAME</h1>
                    <br>
                    <div class="carousel-container">
                        <div class="tab-buttons">
                            <div class="tabs">
                                <button class="tab active btn" onclick="goToSlide(0)">Campaign</button>
                                <button class="tab btn" onclick="goToSlide(1)">Challenges</button>
                                <button class="tab btn" onclick="goToSlide(2)">Shop</button>
                                <button class="tab btn" onclick="goToSlide(3)">Leaderboard</button>
                                <button class="tab btn" onclick="goToSlide(4)">Friends</button>
                            </div>
                        </div>
                        <button class="prev" onclick="prevSlide()">
                            <img src="./kepek/balra.png" alt="Previous Slide" class="truck-left">
                        </button>
                        <button class="next" onclick="nextSlide()">
                            <img src="./kepek/jobbra.png" alt="Next Slide " class="truck-right">
                        </button>
                        
                        <div class="carousel">
    
                                

                            <div class="carousel-track">
                                <div class="slide"><img src="./kepek/image_levels.png" alt="Slide 1"></div>
                                <div class="slide"><img src="./kepek/image_challenges.png" alt="Slide 2"></div>
                                <div class="slide"><img src="./kepek/image_shop.png" alt="Slide 3"></div>
                                <div class="slide"><img src="./kepek/image_leaderboard.png" alt="Slide 4"></div>
                                <div class="slide"><img src="./kepek/image_friends.png" alt="Slide 5"></div>
                            </div>
                            


                        </div>
                        
                        <!-- Egyedi képes indikátorok -->
                        <div class="pagination">
                            <ul>
                            <li><img src="./kepek/active.png" class="dot active" onclick="goToSlide(0)"></li>
                            <li><img src="./kepek/notactive.png" class="dot" onclick="goToSlide(1)"></li>
                            <li><img src="./kepek/notactive.png" class="dot" onclick="goToSlide(2)"></li>
                            <li><img src="./kepek/notactive.png" class="dot" onclick="goToSlide(3)"></li>
                            <li><img src="./kepek/notactive.png" class="dot" onclick="goToSlide(4)"></li>
                            
                            </ul>
                            
                            
                        </div>
                        
                    </div>
                </div>


            </section>
             <div class="download-bar">
           🚀 Ready to challenge your brain? 
<a href="https://www.tangletrack.com/TangleTrack.apk" download><span class="mobile-break">Download Tangle Track now!</span></a>
</div>
            <footer id="contact">
                <div class="footer-container">
                    <div class="social-links">
                        <a target="_blank" href="https://www.facebook.com/profile.php?id=61566264156754"><img
                                src="./kepek/facebook_logo.avif" alt="Facebook"></a>
                        <a target="_blank" href="https://www.x.com/Tangle_Track"><img src="./kepek/twitter_logo.avif"
                                alt="Twitter"></a>
                        <a target="_blank" href="https://www.youtube.com/@TangleTrack"><img
                                src="./kepek/youtube_logo.png" alt="YouTube"></a>
                        <a target="_blank" href="https://www.instagram.com/tangletrack/"><img
                                src="./kepek/insta_logo.jpg" alt="Instagram"></a>
                    </div>
                    <div class="footer-buttons">
                        <a href="./documents/terms_of_service_tangletrack_en.pdf" target="_blank">Terms of Service</a>
                        <a href="./documents/privacy_policy_tangletrack_en.pdf" target="_blank">Privacy Policy</a>
                        <a href="./documents/help_and_support_tangletrack_en.pdf" target="_blank">Help & Support</a>
                    </div>
                    <p>Hungary</p>
                    <p></p>
                    <p>Brainboost Interactive 2025. All Rights Reserved.</p>
                </div>
            </footer>

        </div>
    </div>
    <script>

    </script>

</body>

</html>