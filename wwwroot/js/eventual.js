document.addEventListener('DOMContentLoaded', function () {

    // ── Materialize Init ─────────────────────────────
    M.AutoInit();

    // ── Sidenav ──────────────────────────────────────
    const sidenavElems = document.querySelectorAll('.sidenav');
    M.Sidenav.init(sidenavElems);

    // ── Select ───────────────────────────────────────
    const selectElems = document.querySelectorAll('select');
    M.FormSelect.init(selectElems);

    // ── Datepicker ───────────────────────────────────
    const dateElems = document.querySelectorAll('.datepicker');
    M.Datepicker.init(dateElems, {
        format: 'yyyy-mm-dd',
        i18n: {
            months: ['Enero','Febrero','Marzo','Abril','Mayo','Junio','Julio','Agosto','Septiembre','Octubre','Noviembre','Diciembre'],
            monthsShort: ['Ene','Feb','Mar','Abr','May','Jun','Jul','Ago','Sep','Oct','Nov','Dic'],
            weekdays: ['Domingo','Lunes','Martes','Miércoles','Jueves','Viernes','Sábado'],
            weekdaysShort: ['Do','Lu','Ma','Mi','Ju','Vi','Sa'],
            weekdaysAbbrev: ['D','L','M','M','J','V','S']
        }
    });

    // ── Timepicker ───────────────────────────────────
    const timeElems = document.querySelectorAll('.timepicker');
    M.Timepicker.init(timeElems, { twelveHour: false });

    // ── Auto-dismiss toast after 4s ──────────────────
    const toastContainer = document.querySelector('.af-toast-container');
    if (toastContainer) {
        setTimeout(() => {
            toastContainer.style.opacity = '0';
            toastContainer.style.transition = 'opacity .5s';
            setTimeout(() => toastContainer.remove(), 500);
        }, 4000);
    }

    // ── Image preview on URL input ───────────────────
    const imgInput = document.getElementById('imageUrlInput');
    const imgPreview = document.getElementById('imagePreview');
    if (imgInput && imgPreview) {
        imgInput.addEventListener('input', function () {
            const url = this.value.trim();
            if (url) {
                imgPreview.src = url;
                imgPreview.style.display = 'block';
                imgPreview.onerror = () => {
                    imgPreview.style.display = 'none';
                };
            } else {
                imgPreview.style.display = 'none';
            }
        });
        // Trigger on load if editing
        if (imgInput.value) imgInput.dispatchEvent(new Event('input'));
    }

    // ── Card entrance animation ───────────────────────
    const cards = document.querySelectorAll('.af-card, .af-table-wrap');
    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry, i) => {
            if (entry.isIntersecting) {
                setTimeout(() => {
                    entry.target.style.opacity = '1';
                    entry.target.style.transform = 'translateY(0)';
                }, i * 60);
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.05 });

    cards.forEach(card => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(20px)';
        card.style.transition = 'opacity .4s ease, transform .4s ease';
        observer.observe(card);
    });
});
