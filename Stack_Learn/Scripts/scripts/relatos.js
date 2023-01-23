const relatos = document.querySelectorAll(".relato-step")


const relato1 = document.getElementById("relato1")
const relato2 = document.getElementById("relato2")
const relato3 = document.getElementById("relato3")
const relato4 = document.getElementById("relato4")

const btn1 = document.getElementById("1")
const btn2 = document.getElementById("2")
const btn3 = document.getElementById("3")
const btn4 = document.getElementById("4")

const ballStep = document.querySelectorAll(".step");

const btnRelato = document.querySelectorAll(".btn-relato");

btnRelato.forEach(btn => {
    btn.addEventListener("click", () => {
        if (btn.id == "1") {
            console.log("hi");
            relatoFlex(relato1);
            relatoNone(relato2);
            relatoNone(relato3);
            relatoNone(relato4);

            updateBallSteps(btn1);
            updateBallStepsBack(btn3);
            updateBallStepsBack(btn2);
            updateBallStepsBack(btn4);
        }
        if (btn.id == "2") {
            relatoFlex(relato2);
            relatoNone(relato1);
            relatoNone(relato3);
            relatoNone(relato4);

            updateBallSteps(btn2);
            updateBallStepsBack(btn3);
            updateBallStepsBack(btn4);
            updateBallStepsBack(btn1);
        }
        if (btn.id == "3") {
            relatoFlex(relato3);
            relatoNone(relato2);
            relatoNone(relato1);
            relatoNone(relato4);

            updateBallSteps(btn3);
            updateBallStepsBack(btn4);
            updateBallStepsBack(btn2);
            updateBallStepsBack(btn1);
        }
        if (btn.id == "4") {
            relatoFlex(relato4);
            relatoNone(relato2);
            relatoNone(relato3);
            relatoNone(relato1);

            updateBallSteps(btn4);
            updateBallStepsBack(btn3);
            updateBallStepsBack(btn2);
            updateBallStepsBack(btn1);

        }
    });
});

function relatoFlex(relato) {
    relato.classList.add("displayFlexRelato");
    relato.classList.remove("relatoAluno");
}

function relatoNone(relato) {
    try {
        relato.classList.add("relatoAluno");
        relato.classList.remove("displayFlexRelato");
    }
    catch (err) {

    }
}

function updateBallSteps(bola) {
    bola.classList.remove("bolauncheck");
    bola.classList.add("bolacheck");
}

function updateBallStepsBack(bola) {
    bola.classList.remove("bolacheck");
    bola.classList.add("bolauncheck");

}
