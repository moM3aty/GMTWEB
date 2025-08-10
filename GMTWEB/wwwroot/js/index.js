window.onload = function () {
    window.scrollTo(0, 0);
    // Initialize language on page load
    const savedLang = localStorage.getItem('lang') || 'en';
    updateLanguage(savedLang);
};

// --- Navbar Collapse on Click (Mobile) ---
document.querySelectorAll('.navbar-collapse .nav-link').forEach(link => {
    link.addEventListener('click', () => {
        if (window.innerWidth < 992) {
            const collapseEl = document.querySelector('.navbar-collapse');
            const bsCollapse = bootstrap.Collapse.getInstance(collapseEl) || new bootstrap.Collapse(collapseEl, { toggle: false });
            bsCollapse.hide();
        }
    });
});

// --- Scroll Indicator Logic ---
// Declare the variable ONCE
const scrollIndicator = document.querySelector('.scroll-indicator');

// Check if the element exists before adding listeners
if (scrollIndicator) {
    // Listener to show/hide the indicator based on scroll position
    window.addEventListener("scroll", () => {
        if (window.scrollY > 100) {
            scrollIndicator.style.opacity = "0";
            scrollIndicator.style.visibility = "hidden";
        } else {
            scrollIndicator.style.opacity = "1";
            scrollIndicator.style.visibility = "visible";
        }
    });

    // Listener for the click event to scroll down
    scrollIndicator.addEventListener('click', () => {
        window.scrollBy({
            top: window.innerHeight / 1.13,
            behavior: 'smooth'
        });
    });
}


// --- Contact Form Logic ---
const contactForm = document.getElementById('contactForm');
if (contactForm) {
    const formInputs = contactForm.querySelectorAll('input, textarea');
    const submitBtn = contactForm.querySelector('.submit-btn');
    const successMessage = document.getElementById('formSuccess');

    const messages = {
        en: { required: "This field is required.", email: "Please enter a valid email address.", name: "Name must be at least 2 characters long.", subject: "Subject must be at least 5 characters long.", message: "Message must be at least 10 characters long." },
        ar: { required: "هذا الحقل مطلوب.", email: "يرجى إدخال بريد إلكتروني صحيح.", name: "يجب أن لا يقل الاسم عن حرفين.", subject: "يجب أن لا يقل الموضوع عن 5 أحرف.", message: "يجب أن لا تقل الرسالة عن 10 أحرف." }
    };

    let currentLangForForm = localStorage.getItem('lang') || 'en';

    function validateField(field) {
        const value = field.value.trim();
        const fieldName = field.getAttribute('name');
        const errorElement = document.getElementById(fieldName.replace('user', '').toLowerCase() + 'Error');
        let isValid = true;
        let errorMessage = '';

        if (value === '') {
            isValid = false;
            errorMessage = messages[currentLangForForm].required;
        }
        // Add other validation rules here...

        if (!isValid) {
            field.classList.add('is-invalid');
            errorElement.textContent = errorMessage;
            errorElement.style.display = 'block';
        } else {
            field.classList.remove('is-invalid');
            field.classList.add('is-valid');
            errorElement.style.display = 'none';
        }
        return isValid;
    }

    function clearFieldError(field) {
        const fieldName = field.getAttribute('name');
        const errorElement = document.getElementById(fieldName.replace('user', '').toLowerCase() + 'Error');
        field.classList.remove('is-invalid');
        if (field.value.trim() !== '') {
            field.classList.add('is-valid');
        } else {
            field.classList.remove('is-valid');
        }
        errorElement.style.display = 'none';
    }

    formInputs.forEach(input => {
        input.addEventListener('blur', function () { validateField(this); });
        input.addEventListener('input', function () { clearFieldError(this); });
    });

    contactForm.addEventListener('submit', function (e) {
        e.preventDefault();
        let isFormValid = true;
        formInputs.forEach(input => {
            if (!validateField(input)) {
                isFormValid = false;
            }
        });

        if (isFormValid) {
            const name = document.querySelector('[name="userName"]').value.trim();
            const email = document.querySelector('[name="userEmail"]').value.trim();
            const subject = document.querySelector('[name="subject"]').value.trim();
            const message = document.querySelector('[name="message"]').value.trim();
            const whatsappMessage = `Hello, I would like to contact you.%0AName: ${encodeURIComponent(name)}%0AEmail: ${encodeURIComponent(email)}%0ASubject: ${encodeURIComponent(subject)}%0AMessage: ${encodeURIComponent(message)}`;
            const phoneNumber = "201149364431";
            window.open(`https://wa.me/${phoneNumber}?text=${whatsappMessage}`, '_blank');
            contactForm.reset();
            formInputs.forEach(input => {
                input.classList.remove('is-valid', 'is-invalid');
            });
            successMessage.classList.remove('d-none');
            setTimeout(() => {
                successMessage.classList.add('d-none');
            }, 5000);
        }
    });
}

// --- Language Switcher Logic ---
const langBtn = document.querySelector('.language-btn');
let currentLang = localStorage.getItem('lang') || 'en';

function updateLanguage(lang) {
    document.querySelectorAll('[data-en][data-ar]').forEach(el => {
        el.textContent = el.getAttribute(`data-${lang}`);
    });

    document.querySelectorAll('[data-placeholder-en][data-placeholder-ar]').forEach(el => {
        el.setAttribute('placeholder', el.getAttribute(`data-placeholder-${lang}`));
    });

    document.documentElement.setAttribute('dir', lang === 'ar' ? 'rtl' : 'ltr');
    document.body.classList.toggle('text-end', lang === 'ar');
    document.body.classList.toggle('text-start', lang !== 'ar');

    const enContent = document.querySelector('.lang-en-content');
    const arContent = document.querySelector('.lang-ar-content');
    if (enContent && arContent) {
        if (lang === 'ar') {
            enContent.style.display = 'none';
            arContent.style.display = 'block';
        } else {
            enContent.style.display = 'block';
            arContent.style.display = 'none';
        }
    }

    if (langBtn) {
        langBtn.textContent = lang === 'en' ? 'العربية' : 'English';
    }

    localStorage.setItem('lang', lang);
    currentLang = lang;
}

if (langBtn) {
    langBtn.addEventListener('click', () => {
        const newLang = currentLang === 'en' ? 'ar' : 'en';
        updateLanguage(newLang);
    });
}

// Initialize AOS library
AOS.init({
    offset: 120,
    duration: 1000,
    easing: 'ease-in-out'
});
