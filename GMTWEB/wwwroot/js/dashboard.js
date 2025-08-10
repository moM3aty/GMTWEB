function previewImage(event) {
    var reader = new FileReader();
    reader.onload = function () {
        var output = document.getElementById('imagePreview');
        output.src = reader.result;
    };
    reader.readAsDataURL(event.target.files[0]);
};

document.addEventListener('DOMContentLoaded', function () {
    if (document.getElementById('particles-js')) {
        particlesJS('particles-js', { "particles": { "number": { "value": 80, "density": { "enable": true, "value_area": 800 } }, "color": { "value": "#ffffff" }, "shape": { "type": "circle", }, "opacity": { "value": 0.5, "random": false, }, "size": { "value": 3, "random": true, }, "line_linked": { "enable": true, "distance": 150, "color": "#ffffff", "opacity": 0.4, "width": 1 }, "move": { "enable": true, "speed": 2, "direction": "none", "random": false, "straight": false, "out_mode": "out", "bounce": false, } }, "interactivity": { "detect_on": "canvas", "events": { "onhover": { "enable": true, "mode": "grab" }, "onclick": { "enable": true, "mode": "push" }, "resize": true }, "modes": { "grab": { "distance": 140, "line_linked": { "opacity": 1 } }, "push": { "particles_nb": 4 } } }, "retina_detect": true });
    }

    const chartFont = "'Poppins', sans-serif";
    Chart.defaults.font.family = chartFont;

    // 1. Websites Line Chart
    if (document.getElementById('websitesChart')) {
        const ctx = document.getElementById('websitesChart').getContext('2d');
        const labels = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        const data = { labels: labels, datasets: [{ label: 'New Websites', data: typeof monthlyWebsiteData !== 'undefined' ? monthlyWebsiteData : [], borderColor: '#4f46e5', backgroundColor: 'rgba(79, 70, 229, 0.1)', fill: true, tension: 0.4, borderWidth: 2 }] };
        new Chart(ctx, { type: 'line', data: data, options: { responsive: true, maintainAspectRatio: false, scales: { y: { beginAtZero: true, ticks: { stepSize: 1 } } }, plugins: { legend: { display: false } } } });
    }

    // 2. Project Types Doughnut Chart
    if (document.getElementById('projectTypesChart')) {
        const ctx = document.getElementById('projectTypesChart').getContext('2d');
        const data = {
            labels: ['Educational', 'Corporate', 'E-commerce', 'Portfolio'],
            datasets: [{
                label: 'Project Types',
                data: [12, 19, 5, 8], // Dummy data
                backgroundColor: ['#4f46e5', '#16a34a', '#f97316', '#64748b'],
                borderColor: '#fff',
                borderWidth: 2
            }]
        };
        new Chart(ctx, { type: 'doughnut', data: data, options: { responsive: true, maintainAspectRatio: false, plugins: { legend: { position: 'bottom' } } } });
    }

    // 3. Blog Posts Bar Chart (NOW DYNAMIC)
    if (document.getElementById('blogPostsChart')) {
        const ctx = document.getElementById('blogPostsChart').getContext('2d');
        const labels = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        const data = {
            labels: labels,
            datasets: [{
                label: 'New Blog Posts',
                data: typeof monthlyBlogPostData !== 'undefined' ? monthlyBlogPostData : [], // <-- Use real data
                backgroundColor: 'rgba(249, 115, 22, 0.7)',
                borderColor: '#f97316',
                borderWidth: 1,
                borderRadius: 4
            }]
        };
        new Chart(ctx, { type: 'bar', data: data, options: { responsive: true, maintainAspectRatio: false, scales: { y: { beginAtZero: true, ticks: { stepSize: 1 } } }, plugins: { legend: { display: false } } } });
    }


    // --- Language Switcher Logic ---
    const langBtn = document.querySelector('.dashboard-language-btn');
    let currentDashboardLang = localStorage.getItem('dashboard_lang') || 'en';

    function updateDashboardLanguage(lang) {
        document.querySelectorAll('[data-en][data-ar]').forEach(el => {
            if (el.hasAttribute(`data-${lang}`)) {
                el.textContent = el.getAttribute(`data-${lang}`);
            }
        });
        document.querySelectorAll('[data-placeholder-en][data-placeholder-ar]').forEach(el => {
            if (el.hasAttribute(`data-placeholder-${lang}`)) {
                el.setAttribute('placeholder', el.getAttribute(`data-placeholder-${lang}`));
            }
        });
        if (langBtn) {
            langBtn.querySelector('span').textContent = lang === 'en' ? 'العربية' : 'English';
        }
        document.documentElement.setAttribute('dir', lang === 'ar' ? 'rtl' : 'ltr');
        document.body.classList.toggle('rtl', lang === 'ar');
        localStorage.setItem('dashboard_lang', lang);
        currentDashboardLang = lang;
    }

    if (langBtn) {
        langBtn.addEventListener('click', () => {
            const newLang = currentDashboardLang === 'en' ? 'ar' : 'en';
            updateDashboardLanguage(newLang);
        });
    }
    updateDashboardLanguage(currentDashboardLang);

    // Active sidebar link logic...
    const currentPageTitle = document.title.split(' - ')[0];
    const sidebarLinks = document.querySelectorAll('.sidebar-nav a');
    sidebarLinks.forEach(link => {
        link.classList.remove('active');
        const linkTextEn = link.querySelector('span')?.getAttribute('data-en');
        const linkTextAr = link.querySelector('span')?.getAttribute('data-ar');
        if (linkTextEn === currentPageTitle || linkTextAr === currentPageTitle) {
            link.classList.add('active');
        }
    });
});
