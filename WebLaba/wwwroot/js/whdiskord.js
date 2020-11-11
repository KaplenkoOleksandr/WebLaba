const whurl = "https://discordapp.com/api/webhooks/773654471422115840/Hljsv1p-FXp7B1aZSUXRqJ8IqidHmsX_xybK4-NhAgjcbFLR6k_zzmEsOJ2bJNEYlRK7";

const msg = {
    "embeds": [{
        "image": {
            "url": "https://miro.medium.com/max/1200/1*mk1-6aYaf_Bes1E3Imhc0A.jpeg"
        }
    }]
};

function countRabbits(){
    fetch(whurl + "?wait=true", {
        "method": "POST", "headers": { "content-type": "application/json" },
        "body": JSON.stringify(msg)
    }).then(a => a.json()).then(console.log);
}


