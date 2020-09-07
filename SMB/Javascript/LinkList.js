function openOrCloseList(topicHeader) {
    mainElement = topicHeader.parentNode
    children = mainElement.childNodes
    for (let i = 0; i < children.length; i++) {
        if (children[i].className == "linkList") {
            if (children[i].style.display != "none") {
                children[i].style.display = "none"
            } else {
                children[i].style.display = ""
            }
        }
    }
}