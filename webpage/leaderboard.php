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
    <link rel="stylesheet" href="./css/telcss.css">
    <link rel="stylesheet" href="./css/leaderboard.css">

    <script src="./js/navbar.js"></script>
    <script src="./js/leaderboard.js"></script>


    <link rel="icon" type="image/x-icon" href="./kepek/vegleges1.4.png">
    <title>TangleBoard</title>
    <style>
        .no-highlight .no-download {
            height: auto !important;
            overflow: visible !important;
        }
        .pagination {
    display: flex;
    justify-content: center;
    align-items: center;
    margin-top: 10px; /* Kis távolság a lista alatt */
    gap: 5px;
}

.page-input {
    width: 40px;
    text-align: center;
    padding: 5px;
    border: 1px solid #ccc;
    border-radius: 5px;
}

.arrow-btn {
    padding: 5px 10px;
    border: none;
    background-color: #007bff;
    color: white;
    cursor: pointer;
    border-radius: 5px;
}

.arrow-btn:disabled {
    background-color: #ccc;
    cursor: not-allowed;
}

/* A ranglisták és az oldaltörő gombok konténere */
#pagination-global, #pagination-local {
    width: 100%;
    display: flex;
    justify-content: center;
}
        
         #leaderboard-section {
        display: flex;
        gap: 20px; /* Kisebb gap a két ranglista között */
    }

    #leaderboard-global, #leaderboard-local {
        flex: 1; /* A két div egyforma szélességgel rendelkezik */
    }
    
    
    </style>
</head>

<body>
    <div class="no-highlight">
        <div class="no-download">
            <nav class="sticky-navbar">
                <ul class="sidebar">
                    <li onclick="hideSidebar()"><button><svg xmlns="http://www.w3.org/2000/svg" height="26px"
                                viewBox="0 -960 960 960" width="26px" fill="#FFFFFF">
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
                        <button onlcick="#">
                            <svg xmlns="http://www.w3.org/2000/svg" height="26px" viewBox="0 -960 960 960" width="26px"
                                fill="#FFFFFF">
                                <path d="M120-240v-80h720v80H120Zm0-200v-80h720v80H120Zm0-200v-80h720v80H120Z" />
                            </svg>
                        </button>
                    </li>
                </ul>
            </nav>
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
<!-- https://flagcdn.com/en/codes.json -->
            <section id="leaderboard">
                <div class="leaderboard-section">
                    <h1 class="tangletrack"><span class="tangle">Tangle Track</span></h1>
                    <hr class="kozep">
                    <h1 class="track">LeaderBoard</h1>
                    <br>
                    
<!-- KÜLSŐ konténer, ami tartalmazza mindkét ranglistát -->
<div class="leaderboards-container">
    <h3 class="global"><span class="tangle"> Global </span> 🌍</h3>
    <!-- Globális ranglista -->
    <div class="leaderboard-grid">
        <div id="leaderboard-global"></div>
        <div class="pagination" id="pagination-global">
            <button id="firstPageGlobal" class="arrow-btn" disabled>&laquo;&laquo;</button>
            <button id="prevPageGlobal" class="arrow-btn" disabled>&laquo;</button>
            <input type="number" id="pageInputGlobal" class="page-input" min="1" />
            <button id="nextPageGlobal" class="arrow-btn">&raquo;</button>
            <button id="lastPageGlobal" class="arrow-btn">&raquo;&raquo;</button>
        </div>
    </div>
<h3 class="global"><span class="track"> Local </span> <span class="flag-place"></span></h3>
    <!-- Helyi ranglista -->
    <div class="leaderboard-grid">
        <div id="leaderboard-local"></div>
        <div class="pagination" id="pagination-local">
            <button id="firstPageLocal" class="arrow-btn" disabled>&laquo;&laquo;</button>
            <button id="prevPageLocal" class="arrow-btn" disabled>&laquo;</button>
            <input type="number" id="pageInputLocal" class="page-input" min="1" />
            <button id="nextPageLocal" class="arrow-btn">&raquo;</button>
            <button id="lastPageLocal" class="arrow-btn">&raquo;&raquo;</button>
        </div>
    </div>
</div>
    
</div>
</section>

<script>
    let currentPageGlobal = 1; // Kezdő oldal a globális ranglistához
    let currentPageLocal = 1;  // Kezdő oldal a helyi ranglistához
    let totalPagesGlobal = 10; // Például 10 oldal a globális ranglistához
    let totalPagesLocal = 10;  // Például 10 oldal a helyi ranglistához
    let leaderboardDataGlobal = []; // Globális ranglista adatai
    let leaderboardDataLocal = [];  // Helyi ranglista adatai

    


    // Ranglisták betöltése
    async function getLeaderboard() {
        const response = await fetch("leader.php"); // A PHP backend hívása
        const data = await response.json();

        if (data && data.data && data.data.data.Leaderboard.length > 0) {
            leaderboardDataGlobal = data.data.data.Leaderboard; // Globális ranglista
            leaderboardDataLocal = data.data.data.Leaderboard;  // Helyi ranglista
            displayLeaderboard();
        }
    }
    
    //Xp átalakítása Lvl-re
    function ConvertXPtoLVL(xp) {
            let level = 1;
            let xpNeeded = 50;  // Az első szinthez szükséges XP
            let xpAccumulated = 0;

            // 1-40. szintek: XP = (szint * 50)
            while (level < 40) {
                xpAccumulated += xpNeeded;
                if (xp < xpAccumulated) break;
                level++;
                xpNeeded = (level + 1) * 50;
            }

            // 41-60. szintek: XP = (szint * 100)
            while (level >= 40 && level < 60) {
                xpNeeded = (level + 1) * 100;
                xpAccumulated += xpNeeded;
                if (xp < xpAccumulated) break;
                level++;
            }

            // 61-100. szintek: XP = (szint * 200)
            while (level >= 60 && level < 100) {
                xpNeeded = (level + 1) * 200;
                xpAccumulated += xpNeeded;
                if (xp < xpAccumulated) break;
                level++;
            }

            return level;
        }


    // Ranglisták megjelenítése
    function displayLeaderboard() {
        displayGlobalLeaderboard(currentPageGlobal);
        displayLocalLeaderboard(currentPageLocal);
        updatePagination();
    }

    // Globális ranglista megjelenítése
    function displayGlobalLeaderboard(page) {
        const list = document.getElementById("leaderboard-global");
        list.innerHTML = ""; // Kiüríti a listát az új adatok megjelenítése előtt

        const startIndex = (page - 1) * 5;
        const endIndex = startIndex + 5;
        const currentPageData = leaderboardDataGlobal.slice(startIndex, endIndex);

        currentPageData.forEach(player => {
            const playerDiv = document.createElement("div");
            playerDiv.classList.add("minta");

            let trophyImage = '';
            if (player.Position === 0) {
                trophyImage = '<img src="./kepek/firstplace.png" alt="1st Place" class="trophy">';
            } else if (player.Position === 1) {
                trophyImage = '<img src="./kepek/secondplace.png" alt="2nd Place" class="trophy">';
            } else if (player.Position === 2) {
                trophyImage = '<img src="./kepek/thirdplace.png" alt="3rd Place" class="trophy">';
            } else {
                trophyImage = `<div class="trophy-container">
                    <img src="./kepek/helyezet.png" alt="Trophy" class="trophy">
                    <span class="trophy-text">#${player.Position + 1}</span>
                </div>`;
            }

            playerDiv.innerHTML = `
                <div style="position: relative;">
                    <img src="./kepek/Portrait_Placeholder.png" alt="Profile Picture" class="profile-pic">
                    <div class="rank-badge">
                        <img src="./kepek/star.png" alt="Rank" class="overlay">
                        <span class="rank-number">${ConvertXPtoLVL(player.StatValue)}</span>
                    </div>
                </div>
                <div class="player-info">
                    <div class="player-name">${player.DisplayName || "Névtelen játékos"}</div>
                     <img src="https://flagcdn.com/16x12/ar.png" srcset="https://flagcdn.com/32x24/ar.png 2x, https://flagcdn.com/48x36/ar.png 3x" alt="AR" class="flag">
                </div>
                ${trophyImage}
            `;
            
            list.appendChild(playerDiv);
        });
    }

    // Helyi ranglista megjelenítése
    function displayLocalLeaderboard(page) {
        const list = document.getElementById("leaderboard-local");
        list.innerHTML = ""; // Kiüríti a listát az új adatok megjelenítése előtt

        const startIndex = (page - 1) * 5;
        const endIndex = startIndex + 5;
        const currentPageData = leaderboardDataLocal.slice(startIndex, endIndex);

        currentPageData.forEach(player => {
            const playerDiv = document.createElement("div");
            playerDiv.classList.add("minta");

            let trophyImage = '';
            if (player.Position === 0) {
                trophyImage = '<img src="./kepek/firstplace.png" alt="1st Place" class="trophy">';
            } else if (player.Position === 1) {
                trophyImage = '<img src="./kepek/secondplace.png" alt="2nd Place" class="trophy">';
            } else if (player.Position === 2) {
                trophyImage = '<img src="./kepek/thirdplace.png" alt="3rd Place" class="trophy">';
            } else {
                trophyImage = `<div class="trophy-container">
                    <img src="./kepek/helyezet.png" alt="Trophy" class="trophy">
                    <span class="trophy-text">#${player.Position + 1}</span>
                </div>`;
            }

            playerDiv.innerHTML = `
                <div style="position: relative;">
                    <img src="./kepek/Portrait_Placeholder.png" alt="Profile Picture" class="profile-pic">
                    <div class="rank-badge">
                        <img src="./kepek/star.png" alt="Rank" class="overlay">
                        <span class="rank-number">${ConvertXPtoLVL(player.StatValue)}</span>
                    </div>
                </div>
                <div class="player-info">
                    <div class="player-name">${player.DisplayName || "Névtelen játékos"}</div>
                </div>
                ${trophyImage}
            `;
            
            list.appendChild(playerDiv);
        });
    }

    // Oldal navigáció frissítése
    function updatePagination() {
        // Globális ranglistánál
        document.getElementById("prevPageGlobal").disabled = currentPageGlobal === 1;
        document.getElementById("firstPageGlobal").disabled = currentPageGlobal === 1;
        document.getElementById("nextPageGlobal").disabled = currentPageGlobal === totalPagesGlobal;
        document.getElementById("lastPageGlobal").disabled = currentPageGlobal === totalPagesGlobal;
        document.getElementById("pageInputGlobal").value = currentPageGlobal;

        // Helyi ranglistánál
        document.getElementById("prevPageLocal").disabled = currentPageLocal === 1;
        document.getElementById("firstPageLocal").disabled = currentPageLocal === 1;
        document.getElementById("nextPageLocal").disabled = currentPageLocal === totalPagesLocal;
        document.getElementById("lastPageLocal").disabled = currentPageLocal === totalPagesLocal;
        document.getElementById("pageInputLocal").value = currentPageLocal;
    }

    // Handle navigation for Global Leaderboard
    document.getElementById("prevPageGlobal").addEventListener("click", () => {
        if (currentPageGlobal > 1) {
            currentPageGlobal--;
            displayLeaderboard();
        }
    });

    document.getElementById("nextPageGlobal").addEventListener("click", () => {
        if (currentPageGlobal < totalPagesGlobal) {
            currentPageGlobal++;
            displayLeaderboard();
        }
    });

    document.getElementById("firstPageGlobal").addEventListener("click", () => {
        currentPageGlobal = 1;
        displayLeaderboard();
    });

    document.getElementById("lastPageGlobal").addEventListener("click", () => {
        currentPageGlobal = totalPagesGlobal;
        displayLeaderboard();
    });

    document.getElementById("pageInputGlobal").addEventListener("change", (e) => {
        const pageNumber = parseInt(e.target.value);
        if (pageNumber >= 1 && pageNumber <= totalPagesGlobal) {
            currentPageGlobal = pageNumber;
            displayLeaderboard();
        }
    });

    // Handle navigation for Local Leaderboard
    document.getElementById("prevPageLocal").addEventListener("click", () => {
        if (currentPageLocal > 1) {
            currentPageLocal--;
            displayLeaderboard();
        }
    });

    document.getElementById("nextPageLocal").addEventListener("click", () => {
        if (currentPageLocal < totalPagesLocal) {
            currentPageLocal++;
            displayLeaderboard();
        }
    });

    document.getElementById("firstPageLocal").addEventListener("click", () => {
        currentPageLocal = 1;
        displayLeaderboard();
    });

    document.getElementById("lastPageLocal").addEventListener("click", () => {
        currentPageLocal = totalPagesLocal;
        displayLeaderboard();
    });

    document.getElementById("pageInputLocal").addEventListener("change", (e) => {
        const pageNumber = parseInt(e.target.value);
        if (pageNumber >= 1 && pageNumber <= totalPagesLocal) {
            currentPageLocal = pageNumber;
            displayLeaderboard();
        }
    });

    // Initial call to load leaderboard data
    document.addEventListener("DOMContentLoaded", getLeaderboard);
</script>

 
          <!-- https://flagcdn.com/en/codes.json -->
            
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