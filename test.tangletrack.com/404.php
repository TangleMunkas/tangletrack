<?php
// 404.php
http_response_code(404); // Beállítja a HTTP 404 státuszkódot
?>
<!DOCTYPE html>
<html lang="hu">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>404 - Az oldal nem található</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            color: #333;
            margin: 0;
            padding: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            text-align: center;
        }
        .container {
            max-width: 600px;
            padding: 20px;
        }
        h1 {
            font-size: 100px;
            color: #555;
        }
        p {
            font-size: 20px;
            margin: 20px 0;
        }
        a {
            text-decoration: none;
            background-color: #007BFF;
            color: #fff;
            padding: 10px 20px;
            border-radius: 5px;
            font-size: 18px;
        }
        a:hover {
            background-color: #0056b3;
        }
    </style>
</head>
<body>
    <div class="container">
        <h1>404</h1>
        <p>Az oldal, amit keresel, nem található!</p>
        <a href="/">Vissza a kezdőlapra</a>
    </div>
</body>
</html>
