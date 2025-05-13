function FetchPartial(url, target){
    fetch(url).then(data => {return data.text()}).then(body => {
        target.innerHTML = body;
        var scripts = target.querySelectorAll('script');
        for (let i = 0; i < scripts.length; i++) {
            if (scripts[i].type !== "text/x-template") {
                eval(scripts[i].innerHTML);
            }
        }
    });
}
