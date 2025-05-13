<?php
    session_start(); // Session indítása az oldal tetején
?>

<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="UTF-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        
        <title>Tangle Track</title>
        <meta name="description" content="Tangle Track is a challenging traffic puzzle game where players must untangle gridlocked cars to solve intricate levels. Enjoy multiple game modes including campaign, online 1v1 matches, and a level editor. Free to play on Android with rewards for optional ads and in-app purchases. Compete on global and local leaderboards for the top spot!">
        <meta name="keywords" content="Tangle Track, Rush Hour game, Traffic Jam, Traffic puzzle, mobile game, Logic game, Challenging puzzles, Strategy game, Relaxing yet tricky game, Family-friendly puzzle game, Android, Tangle Trace, Trangle Track, Tangletrack">
        <link rel="apple-touch-icon" sizes="180x180" href="/public/resources/apple-touch-icon.png">
        <link rel="icon" type="image/png" sizes="32x32" href="/public/resources/favicon-32x32.png">
        <link rel="icon" type="image/png" sizes="16x16" href="/public/resources/favicon-16x16.png">
        <link rel="manifest" href="/site.webmanifest">
        <meta name="robots" content="index, follow">
        <link rel="stylesheet" href="styles.css">
    </head>
    
    <body onload="betoltve()">
        <br><br><br>
        <center><h1>Tangle Track is coming in 2025 May!</h1></center>
        <div class="lebego-nagy">
            <center><h2>Follow us on social media for more! ❤</h2></center>
        </div>
        <br><br><br>
        
        <div class="kozossegi-linkek-div">
            <a onclick="trackClick('Instagram')" href="https://www.instagram.com/tangletrack/" target="_blank">
                <img id="tipus_a" class="kozossegi_logo" src="/public/resources/insta_logo.jpg" title="Instagram" alt="Instagram logo" draggable="false">
            </a>
                
            <a onclick="trackClick('TikTok')" href="https://www.tiktok.com/@tangletrack" target="_blank">
                <img id="tipus_b" class="kozossegi_logo" src="/public/resources/tiktok_logo.png" title="TikTok" alt="TikTok logo" draggable="false">
            </a>
                
            <a onclick="trackClick('Facebook')" href="https://www.facebook.com/profile.php?id=61566264156754" target="_blank">
                <img id="tipus_a" class="kozossegi_logo" src="/public/resources/facebook_logo.avif" title="Facebook" alt="facebook logo" draggable="false">
            </a>
            
            <a onclick="trackClick('X (Twitter)')" href="https://www.x.com/Tangle_Track" target="_blank">
                <img id="tipus_b" class="kozossegi_logo" src="/public/resources/twitter_logo.avif" title="X (Twitter)" alt="X (Twitter) logo" draggable="false">
            </a>
            
            <a onclick="trackClick('Discord')" href="https://www.discord.gg/u88fFt5xRU" target="_blank">
                <img id="tipus_a" class="kozossegi_logo" src="/public/resources/discord_logo.jpg" title="Discord" alt="Discord logo" draggable="false">
            </a>
            
            <a onclick="trackClick('YouTube')" href="https://www.youtube.com/@TangleTrack" target="_blank">
                <img id="tipus_b" class="kozossegi_logo" src="/public/resources/youtube_logo.png" title="YouTube" alt="YouTube logo" draggable="false">
            </a>
            <a href="https://smartin.hu/"></a>
        </div>   
        
        <br><br><br><br><br><br>
        <center>
            <a onclick="loadIframe(); trackDemoGame()" id="try-game-button" class="try-game-button" alt="Load demo game button">Load demo game</a>
        </center>
        <br><br><br>
        <center>
            <div id="iframe-container" alt="demo game"></div>
        </center>
    
        <script src="script.js"></script>
    </body>
</html>
