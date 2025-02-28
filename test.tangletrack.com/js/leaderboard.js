async function getCountryCode() {
    try {
        const response = await fetch("https://ipwhois.app/json/");
        const data = await response.json();
        if (data.country_code) {
            setCountryFlag(data.country_code.toLowerCase());
        } else {
            console.error("Invalid response:", data);
        }
    } catch (error) {
        console.error("Error fetching location:", error);
    }
}

function setCountryFlag(countryCode) {
    const placeholderElement = document.querySelector(".global .flag-place");
    if (placeholderElement) {
        placeholderElement.innerHTML = `  <img src="https://flagcdn.com/24x18/${countryCode}.png" 
        alt="Country Flag" style="vertical-align: middle;">`;
     
     // **Stílusbeállítások a JavaScript-ben**
        flagImg.style.width = "1.8rem";  // Szélesség megegyezik a szöveggel
        flagImg.style.height = "1.8rem"; // Magasság megegyezik a szöveggel
        flagImg.style.verticalAlign = "middle"; // Középre igazítás
        flagImg.style.marginLeft = "0.5rem"; // Kis távolság a szövegtől
        flagImg.style.backgroundColor = "#ccc"; // Háttérszín a biztos megjelenítéshez

        // **Korábbi tartalom törlése és új zászló beszúrása**
        flagElement.innerHTML = "";
        flagElement.appendChild(flagImg);
    }
}

// Hívjuk meg a funkciót a böngésző betöltése után
getCountryCode();