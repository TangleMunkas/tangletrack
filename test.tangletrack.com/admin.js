document.addEventListener("DOMContentLoaded", () => {
    const startTime = Date.now();  // Mikor nyitotta meg az oldalt
    const sessionId = localStorage.getItem("session_id") || generateSessionId();
    const pageUrl = window.location.pathname;  // Az oldal URL-je

    window.addEventListener("beforeunload", () => {
        const endTime = Date.now();
        const duration = Math.round((endTime - startTime) / 1000); // Időtartam másodpercben

        fetch("/track_visit.php", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                session_id: sessionId,
                page_url: pageUrl,
                visit_start: startTime,
                visit_end: endTime,
                duration: duration
            })
        });
    });

    function generateSessionId() {
        const id = Math.random().toString(36).substr(2, 10);
        localStorage.setItem("session_id", id);
        return id;
    }
});
