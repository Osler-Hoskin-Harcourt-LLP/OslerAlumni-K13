const TranscriptToggle = (() => {
    const buttons = Array.from(document.querySelectorAll('.c-video-toggle'));

    if(buttons.length > 0 ) {
        buttons.forEach(button => {
            let id = button.dataset.id;
            button.addEventListener("click", () => {
                let transcript =  document.querySelector(`[data-transcript-id=${id}]`);
                transcript.classList.toggle('show');
                if (transcript.classList.contains('show')) {
                    button.classList.add('open');
                    button.setAttribute('aria-expanded', true);
                    transcript.setAttribute('aria-hidden', false);
                    transcript.setAttribute('tabindex', 0);
                }
                else {
                    button.classList.remove('open');
                    button.setAttribute('aria-expanded', false);
                    transcript.setAttribute('aria-hidden', true);
                    transcript.setAttribute('tabindex', -1);
                }
            });

        })
    }


})();

export default TranscriptToggle;