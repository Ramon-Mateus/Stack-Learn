
$('.filter-btn').on('click', function () {
    let type = $(this).attr('id');
    let boxes = $('.project-box')

    $('.main-btn').removeClass('active');
    $(this).addClass('active');

    if (type == 'all-btn') {
        eachBoxes('all', boxes);
    } else if (type == 'OAB-btn') {
        eachBoxes('OAB', boxes);
    } else if (type == 'ENEM-btn') {
        eachBoxes('ENEM', boxes);
    } else if (type == 'Vestibulares-btn') {
        eachBoxes('Vestibulares', boxes);
    }
});

function eachBoxes(type, boxes) {
    if (type == 'all') {
        boxes.style.display = 'block';

    } else {
        $(boxes).each(function () {
            if (!$(this).hasClass(type)) {
                this.style.display = 'none';

            } else {
                this.style.display = 'block';
            }
        });
    }
}
 