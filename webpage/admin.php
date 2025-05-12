<?php
session_start();

// Admin jelszó beállítása (cseréld le egy biztonságosabb megoldásra!)
$admin_password = 'Tan20gle25T0r3a0c9k';

// Ha nincs bejelentkezve, kérje a jelszót
if (!isset($_SESSION['logged_in'])) {
    if ($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_POST['password'])) {
        if ($_POST['password'] === $admin_password) {
            $_SESSION['logged_in'] = true;
        } else {
            echo "<p>Hibás jelszó!</p>";
        }
    }
    
    if (!isset($_SESSION['logged_in'])) {
        echo '<form method="POST"><input type="password" name="password" placeholder="Admin jelszó" required><button type="submit">Belépés</button></form>';
        exit;
    }
}

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

// Szűrés és keresés
$where = "";
$params = [];
if (!empty($_GET['ip'])) {
    $where .= " AND ip LIKE ?";
    $params[] = '%' . $_GET['ip'] . '%';
}
if (!empty($_GET['country'])) {
    $where .= " AND country LIKE ?";
    $params[] = '%' . $_GET['country'] . '%';
}
if (!empty($_GET['date'])) {
    $where .= " AND DATE(visit_date) = ?";
    $params[] = $_GET['date'];
}

$sql = "SELECT * FROM visitors WHERE 1 $where ORDER BY visit_date DESC";
$stmt = $pdo->prepare($sql);
$stmt->execute($params);
$visitors = $stmt->fetchAll(PDO::FETCH_ASSOC);
?>
<!DOCTYPE html>
<html>
<head>
    <title>Admin - Látogatói adatok</title>
    <style>
        table { border-collapse: collapse; width: 100%; }
        th, td { border: 1px solid black; padding: 8px; text-align: left; }
        th { background-color: #f2f2f2; }
    </style>
</head>
<body>
    <h1>Admin - Látogatói adatok</h1>
    <form method="GET">
        <input type="text" name="ip" placeholder="IP cím" value="<?php echo $_GET['ip'] ?? ''; ?>">
        <input type="text" name="country" placeholder="Ország" value="<?php echo $_GET['country'] ?? ''; ?>">
        <input type="date" name="date" value="<?php echo $_GET['date'] ?? ''; ?>">
        <button type="submit">Keresés</button>
    </form>
    <table>
    <tr>
        <th>IP cím</th>
        <th>Ország</th>
        <th>Böngésző</th>
        <th>Eszköz</th>
        <th>Oldal</th>
        <th>Dátum</th>
    </tr>
    <?php foreach ($visitors as $visitor): ?>
    <tr>
        <td><?php echo htmlspecialchars($visitor['ip']); ?></td>
        <td><?php echo htmlspecialchars($visitor['country']); ?></td>
        <td><?php echo htmlspecialchars($visitor['browser']); ?></td>
        <td><?php echo htmlspecialchars($visitor['device']); ?></td>
        <td><?php echo htmlspecialchars($visitor['page']); ?></td>
        <td><?php echo htmlspecialchars($visitor['visit_date']); ?></td>
    </tr>
    <?php endforeach; ?>
</table>
    <form method="POST" action="logout.php">
        <button type="submit">Kijelentkezés</button>
    </form>
</body>
</html>
