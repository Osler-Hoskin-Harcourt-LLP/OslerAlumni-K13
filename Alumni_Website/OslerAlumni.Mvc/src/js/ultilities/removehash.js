const RemoveHash = (() => {

    const hashNodes = Array.from(document.querySelectorAll('[data-hash]'));

    return {
        init: () => {
            if (hashNodes.length > 0) {
                hashNodes.forEach((hashNode) => {
                    const hashHref = hashNode.getAttribute('href').substr(1);

                    hashNode.onclick = (e) => {
                        e.preventDefault();
                        document.getElementById(hashHref).focus();
                    };
                });
            }
        }
    }
})();

export default RemoveHash;
