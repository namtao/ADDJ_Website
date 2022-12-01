function fixform() {
    if (opener.document.getElementById("aspnetForm").target != "_blank") return;

    opener.document.getElementById("aspnetForm").target = "";
    opener.document.getElementById("aspnetForm").action = opener.location.href;
    }
