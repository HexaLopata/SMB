function confirmDelete(argumentName, id, path) {
    let modalWindow = document.querySelector(".modalWindow")
    modalWindow.style.display = "block"
    modalWindow.innerHTML = `
        <b class="modalWindowText">Вы уверены, что хотите удалить это?</b>
        <form class="modalWindowButtons" method="post" action="${path}">
            <input type="hidden" name="${argumentName}" value="${id}" />
            <input id="deleteButton" onclick="closeModalWindow()" type="submit" value="Удалить" />
            <button type="button" id="backButton" onclick="closeModalWindow()">Назад</button>
        </form >`
}

function closeModalWindow() {
    let modalWindow = document.querySelector(".modalWindow")
    modalWindow.style.display = "none"
}

let formsWithValidate = document.querySelectorAll(".formWithValidation")

for (let i = 0; i < formsWithValidate.length; i++) {
    formsWithValidate[i].addEventListener("submit", function (event) {
        let textAreas = formsWithValidate[i].querySelectorAll(".textWithEmptyValidation")
        for (let j = 0; j < textAreas.length; j++) {
            if (textAreas[j].value.trim() === "") {
                event.preventDefault();
            }
        }
    })
}