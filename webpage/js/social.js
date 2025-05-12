const form = document.getElementById('comment-form');
const commentsList = document.querySelector('#comments-list ul');

form.addEventListener('submit', (e) => {
  e.preventDefault();
  // Get form values
  const name = document.getElementById('name').value;
  const comment = document.getElementById('comment').value;

  // Create a new comment element
  const li = document.createElement('li');
  li.innerHTML = `<strong>${name}</strong><p>${comment}</p>`;

  // Add the new comment to the list
  commentsList.appendChild(li);

  // Clear the form
  form.reset();
});


document.getElementById("contactForm").addEventListener("submit", function(event) {
    event.preventDefault(); // Ne töltse újra az oldalt

    let formData = new FormData(this);

    fetch("send_email.php", {
        method: "POST",
        body: formData
    })
    .then(response => response.text())
    .then(data => {
        let responseMessage = document.getElementById("responseMessage");

        if (data.includes("success")) {
            responseMessage.innerHTML = "Üzenet sikeresen elküldve!";
            responseMessage.className = "success";
            document.getElementById("contactForm").reset(); // Mezők törlése
        } else {
            responseMessage.innerHTML = "Üzenet küldése sikertelen!<br>Ellenőrizze üzenetét";
            responseMessage.className = "error";
        }

        responseMessage.style.display = "block"; // Üzenet megjelenítése
    })
    .catch(error => {
        let responseMessage = document.getElementById("responseMessage");
        responseMessage.innerHTML = "Hiba történt! Próbálja újra később.";
        responseMessage.className = "error";
        responseMessage.style.display = "block";
    });
});
