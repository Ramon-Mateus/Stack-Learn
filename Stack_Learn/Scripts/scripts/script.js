const prevBtns = document.querySelectorAll(".btn-prev");
const nextBtns = document.querySelectorAll(".btn-next");
const progress = document.getElementById("progress");
const formSteps = document.querySelectorAll(".form-step");

const ballStep = document.querySelectorAll(".step");

let formStepsNum = 0;

nextBtns.forEach(btn => {
    btn.addEventListener("click", () => {
        formStepsNum++;
        updateFormSteps();
        updateBallSteps();
        updateProgressSteps();
    });
});

prevBtns.forEach(btn => {
    btn.addEventListener("click", () => {
        formStepsNum--;
        updateFormStepsBack();
        updateBallStepsBack();
        updateProgressStepsBack();
    });
});

function updateFormSteps() {
    formSteps[formStepsNum].classList.add("form-step-active");
    formSteps[formStepsNum - 1].classList.remove("form-step-active");
}

function updateFormStepsBack() {
    formSteps[formStepsNum].classList.add("form-step-active");
    formSteps[formStepsNum + 1].classList.remove("form-step-active");
}

function updateBallSteps() {
    ballStep[formStepsNum].classList.remove("bolauncheck");
    ballStep[formStepsNum].classList.add("bolacheck");

    ballStep[formStepsNum - 1].style.width = "35px";
    ballStep[formStepsNum - 1].style.height = "35px";

    ballStep[formStepsNum].style.width = "40px";
    ballStep[formStepsNum].style.height = "40px";
    
}

function updateBallStepsBack() {
    ballStep[formStepsNum + 1].classList.remove("bolacheck");
    ballStep[formStepsNum + 1].classList.add("bolauncheck");

    ballStep[formStepsNum + 1].style.width = "35px";
    ballStep[formStepsNum + 1].style.height = "35px";

    ballStep[formStepsNum].style.width = "40px";
    ballStep[formStepsNum].style.height = "40px"
    
}

function updateProgressSteps() {
    progress.style.width = (formStepsNum * 50 + "%")
    
}

function updateProgressStepsBack() {
    progress.style.width = (formStepsNum * 50 + "%")
}

function showCvv() {
    var x = document.getElementById("cvv");
    var y = document.getElementById("icon");

    if (x.type === "password") {
    x.type = "text";
    y.classList.add("check_box");
    y.classList.remove("checkpass");
    } else {
    x.type = "password";
    y.classList.remove("check_box");
    y.classList.add("checkpass");
    }
}