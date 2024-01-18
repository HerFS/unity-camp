<?php

$servername = "localhost";
$username = "root";
$password = "";
$dbname = "playerstats";

// Ceateconnection
$conn = new mysqli($servername, $username, $password, $dbname);

// check connection
if ($conn->connect_error) {
	die("Connection failed: ". $conn->connect_error);
}
echo "Connected successfully, now we will show the users";

$sql = "SELECT id, username, password, level, gold FROM users";

$result = $conn->query($sql);

if ($result->num_rows > 0) {
	// outpub data of each row
	while ($row = $result->fetch_assoc()){
		echo "id: " .$row["id"]. " name: " .$row["username"]. " password: " . $row["password"]. " level: " . $row["level"]. " gold: " . $row["gold"]. "<br>";
	}
} else {
	echo "0 results";
}
$conn->close();

?>