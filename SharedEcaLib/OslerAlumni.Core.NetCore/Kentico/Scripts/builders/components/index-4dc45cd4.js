let e,t,n,l=!1,o=!1,s=!1,r=0,i=!1;const c="undefined"!=typeof window?window:{},a=c.CSS,f=c.document||{head:{}},u={t:0,l:"",jmp:e=>e(),raf:e=>requestAnimationFrame(e),ael:(e,t,n,l)=>e.addEventListener(t,n,l),rel:(e,t,n,l)=>e.removeEventListener(t,n,l)},m=(()=>(f.head.attachShadow+"").indexOf("[native")>-1)(),d=e=>Promise.resolve(e),p=(()=>{try{return new CSSStyleSheet,!0}catch(e){}return!1})(),$={},h=(e,t,n)=>{n&&n.map(([n,l,o])=>{const s=e,r=w(t,o),i=y(n);u.ael(s,l,r,i),(t.o=t.o||[]).push(()=>u.rel(s,l,r,i))})},w=(e,t)=>n=>{256&e.t?e.s[t](n):(e.u=e.u||[]).push([t,n])},y=e=>0!=(2&e),b=new WeakMap,_=e=>"sc-"+e,v={},g=e=>"object"==(e=typeof e)||"function"===e,j=(e,t,...n)=>{let l=null,o=null,s=null,r=!1,i=!1,c=[];const a=t=>{for(let n=0;n<t.length;n++)l=t[n],Array.isArray(l)?a(l):null!=l&&"boolean"!=typeof l&&((r="function"!=typeof e&&!g(l))&&(l=String(l)),r&&i?c[c.length-1].p+=l:c.push(r?k(null,l):l),i=r)};if(a(n),t){t.key&&(o=t.key),t.name&&(s=t.name);{const e=t.className||t.class;e&&(t.class="object"!=typeof e?e:Object.keys(e).filter(t=>e[t]).join(" "))}}const f=k(e,null);return f.$=t,c.length>0&&(f.h=c),f._=o,f.v=s,f},k=(e,t)=>({t:0,g:e,p:t,j:null,h:null,$:null,_:null,v:null}),R={},S=(e,t,n,l,o,s)=>{if(n!==l){let i=ie(e,t),a=t.toLowerCase();if("class"===t){const t=e.classList,o=U(n),s=U(l);t.remove(...o.filter(e=>e&&!s.includes(e))),t.add(...s.filter(e=>e&&!o.includes(e)))}else if("style"===t){for(const t in n)l&&null!=l[t]||(t.includes("-")?e.style.removeProperty(t):e.style[t]="");for(const t in l)n&&l[t]===n[t]||(t.includes("-")?e.style.setProperty(t,l[t]):e.style[t]=l[t])}else if("key"===t);else if("ref"===t)l&&l(e);else if(i||"o"!==t[0]||"n"!==t[1]){const c=g(l);if((i||c&&null!==l)&&!o)try{if(e.tagName.includes("-"))e[t]=l;else{let o=null==l?"":l;"list"===t?i=!1:null!=n&&e[t]==o||(e[t]=o)}}catch(r){}null==l||!1===l?e.removeAttribute(t):(!i||4&s||o)&&!c&&e.setAttribute(t,l=!0===l?"":l)}else t="-"===t[2]?t.slice(3):ie(c,a)?a.slice(2):a[2]+t.slice(3),n&&u.rel(e,t,n,!1),l&&u.ael(e,t,l,!1)}},M=/\s/,U=e=>e?e.split(M):[],L=(e,t,n,l)=>{const o=11===t.j.nodeType&&t.j.host?t.j.host:t.j,s=e&&e.$||v,r=t.$||v;for(l in s)l in r||S(o,l,s[l],void 0,n,t.t);for(l in r)S(o,l,s[l],r[l],n,t.t)},P=(o,r,i,c)=>{let a,u,m,d=r.h[i],p=0;if(l||(s=!0,"slot"===d.g&&(e&&c.classList.add(e+"-s"),d.t|=d.h?2:1)),null!==d.p)a=d.j=f.createTextNode(d.p);else if(1&d.t)a=d.j=f.createTextNode("");else if(a=d.j=f.createElement(2&d.t?"slot-fb":d.g),L(null,d,!1),null!=e&&a["s-si"]!==e&&a.classList.add(a["s-si"]=e),d.h)for(p=0;p<d.h.length;++p)u=P(o,d,p,a),u&&a.appendChild(u);return a["s-hn"]=n,3&d.t&&(a["s-sr"]=!0,a["s-cr"]=t,a["s-sn"]=d.v||"",m=o&&o.h&&o.h[i],m&&m.g===d.g&&o.j&&C(o.j,!1)),a},C=(e,t)=>{u.t|=1;const l=e.childNodes;for(let o=l.length-1;o>=0;o--){const e=l[o];e["s-hn"]!==n&&e["s-ol"]&&(A(e).insertBefore(e,E(e)),e["s-ol"].remove(),e["s-ol"]=void 0,s=!0),t&&C(e,t)}u.t&=-2},O=(e,t,l,o,s,r)=>{let i,c=e["s-cr"]&&e["s-cr"].parentNode||e;for(c.shadowRoot&&c.tagName===n&&(c=c.shadowRoot);s<=r;++s)o[s]&&(i=P(null,l,s,e),i&&(o[s].j=i,c.insertBefore(i,E(t))))},T=(e,t,n,l,s)=>{for(;t<=n;++t)(l=e[t])&&(s=l.j,B(l),o=!0,s["s-ol"]?s["s-ol"].remove():C(s,!0),s.remove())},x=(e,t)=>e.g===t.g&&("slot"===e.g?e.v===t.v:e._===t._),E=e=>e&&e["s-ol"]||e,A=e=>(e["s-ol"]?e["s-ol"]:e).parentNode,D=(e,t)=>{const n=t.j=e.j,l=e.h,o=t.h,s=t.p;let r;null===s?("slot"===t.g||L(e,t,!1),null!==l&&null!==o?((e,t,n,l)=>{let o,s,r=0,i=0,c=0,a=0,f=t.length-1,u=t[0],m=t[f],d=l.length-1,p=l[0],$=l[d];for(;r<=f&&i<=d;)if(null==u)u=t[++r];else if(null==m)m=t[--f];else if(null==p)p=l[++i];else if(null==$)$=l[--d];else if(x(u,p))D(u,p),u=t[++r],p=l[++i];else if(x(m,$))D(m,$),m=t[--f],$=l[--d];else if(x(u,$))"slot"!==u.g&&"slot"!==$.g||C(u.j.parentNode,!1),D(u,$),e.insertBefore(u.j,m.j.nextSibling),u=t[++r],$=l[--d];else if(x(m,p))"slot"!==u.g&&"slot"!==$.g||C(m.j.parentNode,!1),D(m,p),e.insertBefore(m.j,u.j),m=t[--f],p=l[++i];else{for(c=-1,a=r;a<=f;++a)if(t[a]&&null!==t[a]._&&t[a]._===p._){c=a;break}c>=0?(s=t[c],s.g!==p.g?o=P(t&&t[i],n,c,e):(D(s,p),t[c]=void 0,o=s.j),p=l[++i]):(o=P(t&&t[i],n,i,e),p=l[++i]),o&&A(u.j).insertBefore(o,E(u.j))}r>f?O(e,null==l[d+1]?null:l[d+1].j,n,l,i,d):i>d&&T(t,r,f)})(n,l,t,o):null!==o?(null!==e.p&&(n.textContent=""),O(n,null,t,o,0,o.length-1)):null!==l&&T(l,0,l.length-1)):(r=n["s-cr"])?r.parentNode.textContent=s:e.p!==s&&(n.data=s)},F=e=>{let t,n,l,o,s,r,i=e.childNodes;for(n=0,l=i.length;n<l;n++)if(t=i[n],1===t.nodeType){if(t["s-sr"])for(s=t["s-sn"],t.hidden=!1,o=0;o<l;o++)if(i[o]["s-hn"]!==t["s-hn"])if(r=i[o].nodeType,""!==s){if(1===r&&s===i[o].getAttribute("slot")){t.hidden=!0;break}}else if(1===r||3===r&&""!==i[o].textContent.trim()){t.hidden=!0;break}F(t)}},W=[],q=e=>{let t,n,l,s,r,i,c=0,a=e.childNodes,f=a.length;for(;c<f;c++){if(t=a[c],t["s-sr"]&&(n=t["s-cr"]))for(l=n.parentNode.childNodes,s=t["s-sn"],i=l.length-1;i>=0;i--)n=l[i],n["s-cn"]||n["s-nr"]||n["s-hn"]===t["s-hn"]||(N(n,s)?(r=W.find(e=>e.k===n),o=!0,n["s-sn"]=n["s-sn"]||s,r?r.R=t:W.push({R:t,k:n}),n["s-sr"]&&W.map(e=>{N(e.k,n["s-sn"])&&(r=W.find(e=>e.k===n),r&&!e.R&&(e.R=r.R))})):W.some(e=>e.k===n)||W.push({k:n}));1===t.nodeType&&q(t)}},N=(e,t)=>1===e.nodeType?null===e.getAttribute("slot")&&""===t||e.getAttribute("slot")===t:e["s-sn"]===t||""===t,B=e=>{e.$&&e.$.ref&&e.$.ref(null),e.h&&e.h.map(B)},H=(e,t)=>{t&&!e.S&&t["s-p"].push(new Promise(t=>e.S=t))},V=(e,t)=>{if(e.t|=16,4&e.t)return void(e.t|=512);const n=e.s,l=()=>z(e,n,t);let o;return H(e,e.M),t&&(e.t|=256,e.u&&(e.u.map(([e,t])=>K(n,e,t)),e.u=null),o=K(n,"componentWillLoad")),Q(o,()=>_e(l))},z=(r,i,c)=>{const a=r.U,d=a["s-rc"];c&&(e=>{const t=e.L,n=e.U,l=t.t,o=((e,t)=>{let n=_(t.P),l=ue.get(n);if(e=11===e.nodeType?e:f,l)if("string"==typeof l){let t,o=b.get(e=e.head||e);o||b.set(e,o=new Set),o.has(n)||(t=f.createElement("style"),t.innerHTML=l,e.insertBefore(t,e.querySelector("link")),o&&o.add(n))}else e.adoptedStyleSheets.includes(l)||(e.adoptedStyleSheets=[...e.adoptedStyleSheets,l]);return n})(m&&n.shadowRoot?n.shadowRoot:n.getRootNode(),t);10&l&&(n["s-sc"]=o,n.classList.add(o+"-h"))})(r),((r,i)=>{const c=r.U,a=r.L,d=r.C||k(null,null),p=($=i)&&$.g===R?i:j(null,null,i);var $;if(n=c.tagName,p.g=null,p.t|=4,r.C=p,p.j=d.j=c.shadowRoot||c,e=c["s-sc"],t=c["s-cr"],l=m&&0!=(1&a.t),o=!1,D(d,p),s){let e,t,n,l,o,s;u.t|=1,q(p.j);let r=0;for(;r<W.length;r++)e=W[r],t=e.k,t["s-ol"]||(n=f.createTextNode(""),n["s-nr"]=t,t.parentNode.insertBefore(t["s-ol"]=n,t));for(r=0;r<W.length;r++)if(e=W[r],t=e.k,e.R){for(l=e.R.parentNode,o=e.R.nextSibling,n=t["s-ol"];n=n.previousSibling;)if(s=n["s-nr"],s&&s["s-sn"]===t["s-sn"]&&l===s.parentNode&&(s=s.nextSibling,!s||!s["s-nr"])){o=s;break}(!o&&l!==t.parentNode||t.nextSibling!==o)&&t!==o&&(!t["s-hn"]&&t["s-ol"]&&(t["s-hn"]=t["s-ol"].parentNode.nodeName),l.insertBefore(t,o))}else 1===t.nodeType&&(t.hidden=!0);u.t&=-2}o&&F(p.j),W.length=0})(r,G(i)),r.t&=-17,r.t|=2,d&&(d.map(e=>e()),a["s-rc"]=void 0);{const e=a["s-p"],t=()=>I(r);0===e.length?t():(Promise.all(e).then(t),r.t|=4,e.length=0)}},G=e=>{try{e=e.render&&e.render()}catch(t){ce(t)}return e},I=e=>{const t=e.U,n=e.s,l=e.M;64&e.t?K(n,"componentDidUpdate"):(e.t|=64,X(t),K(n,"componentDidLoad"),e.O(t),l||J()),e.T(t),e.S&&(e.S(),e.S=void 0),512&e.t&&ye(()=>V(e,!1)),e.t&=-517},J=()=>{X(f.documentElement),u.t|=2},K=(e,t,n)=>{if(e&&e[t])try{return e[t](n)}catch(l){ce(l)}},Q=(e,t)=>e&&e.then?e.then(t):t(),X=e=>e.classList.add("hydrated"),Y=(e,t,n)=>{if(t.A){e.watchers&&(t.D=e.watchers);const l=Object.entries(t.A),o=e.prototype;if(l.map(([e,[l]])=>{31&l||2&n&&32&l?Object.defineProperty(o,e,{get(){return t=e,oe(this).F.get(t);var t},set(n){((e,t,n,l)=>{const o=oe(this),s=o.F.get(t),r=o.t,i=o.s;var c,a;if(a=l.A[t][0],n=null==(c=n)||g(c)?c:4&a?"false"!==c&&(""===c||!!c):2&a?parseFloat(c):1&a?String(c):c,!(8&r&&void 0!==s||n===s)&&(o.F.set(t,n),i)){if(l.D&&128&r){const e=l.D[t];e&&e.map(e=>{try{i[e](n,s,t)}catch(l){ce(l)}})}2==(18&r)&&V(o,!1)}})(0,e,n,t)},configurable:!0,enumerable:!0}):1&n&&64&l&&Object.defineProperty(o,e,{value(...t){const n=oe(this);return n.W.then(()=>n.s[e](...t))}})}),1&n){const t=new Map;o.attributeChangedCallback=function(e,n,l){u.jmp(()=>{const n=t.get(e);this[n]=(null!==l||"boolean"!=typeof this[n])&&l})},e.observedAttributes=l.filter(([e,t])=>15&t[0]).map(([e,n])=>{const l=n[1]||e;return t.set(l,e),l})}}return e},Z=(e,t={})=>{const n=[],l=t.exclude||[],o=c.customElements,s=f.head,r=s.querySelector("meta[charset]"),i=f.createElement("style"),a=[];let d,$=!0;Object.assign(u,t),u.l=new URL(t.resourcesUrl||"./",f.baseURI).href,t.syncQueue&&(u.t|=4),e.map(e=>e[1].map(t=>{const s={t:t[0],P:t[1],A:t[2],q:t[3]};s.A=t[2],s.q=t[3],s.D={},!m&&1&s.t&&(s.t|=8);const r=s.P,i=class extends HTMLElement{constructor(e){super(e),re(e=this,s),1&s.t&&(m?e.attachShadow({mode:"open"}):"shadowRoot"in e||(e.shadowRoot=e))}connectedCallback(){d&&(clearTimeout(d),d=null),$?a.push(this):u.jmp(()=>(e=>{if(0==(1&u.t)){const t=oe(e),n=t.L,l=()=>{};if(1&t.t)h(e,t,n.q);else{t.t|=1,12&n.t&&(e=>{const t=e["s-cr"]=f.createComment("");t["s-cn"]=!0,e.insertBefore(t,e.firstChild)})(e);{let n=e;for(;n=n.parentNode||n.host;)if(n["s-p"]){H(t,t.M=n);break}}n.A&&Object.entries(n.A).map(([t,[n]])=>{if(31&n&&e.hasOwnProperty(t)){const n=e[t];delete e[t],e[t]=n}}),ye(()=>(async(e,t,n,l,o)=>{if(0==(32&t.t)){t.t|=32;{if((o=fe(n)).then){const e=()=>{};o=await o,e()}o.isProxied||(n.D=o.watchers,Y(o,n,2),o.isProxied=!0);const e=()=>{};t.t|=8;try{new o(t)}catch(i){ce(i)}t.t&=-9,t.t|=128,e()}const e=_(n.P);if(!ue.has(e)&&o.style){const t=()=>{};let l=o.style;8&n.t&&(l=await __sc_import_components("./shadow-css-14c5e2cb.js").then(t=>t.scopeCss(l,e,!1))),((e,t,n)=>{let l=ue.get(e);p&&n?(l=l||new CSSStyleSheet,l.replace(t)):l=t,ue.set(e,l)})(e,l,!!(1&n.t)),t()}}const s=t.M,r=()=>V(t,!0);s&&s["s-rc"]?s["s-rc"].push(r):r()})(0,t,n))}l()}})(this))}disconnectedCallback(){u.jmp(()=>(()=>{if(0==(1&u.t)){const e=oe(this),t=e.s;e.o&&(e.o.map(e=>e()),e.o=void 0),K(t,"componentDidUnload")}})())}forceUpdate(){(()=>{{const e=oe(this);e.U.isConnected&&2==(18&e.t)&&V(e,!1)}})()}componentOnReady(){return oe(this).N}};s.B=e[0],l.includes(r)||o.get(r)||(n.push(r),o.define(r,Y(i,s,1)))})),i.innerHTML=n+"{visibility:hidden}.hydrated{visibility:inherit}",i.setAttribute("data-styles",""),s.insertBefore(i,r?r.nextSibling:s.firstChild),$=!1,a.length?a.map(e=>e.connectedCallback()):u.jmp(()=>d=setTimeout(J,30))},ee=e=>oe(e).U,te=(e,t,n)=>{const l=ee(e);return{emit:e=>{const o=new CustomEvent(t,{bubbles:!!(4&n),composed:!!(2&n),cancelable:!!(1&n),detail:e});return l.dispatchEvent(o),o}}},ne=(e,t)=>t in $?$[t]:"window"===t?c:"document"===t?f:"isServer"!==t&&"isPrerender"!==t&&("isClient"===t||("resourcesUrl"===t||"publicPath"===t?(()=>{const e=new URL(".",u.l);return e.origin!==c.location.origin?e.href:e.pathname})():"queue"===t?{write:_e,read:be,tick:{then:e=>ye(e)}}:void 0)),le=new WeakMap,oe=e=>le.get(e),se=(e,t)=>le.set(t.s=e,t),re=(e,t)=>{const n={t:0,U:e,L:t,F:new Map};return n.W=new Promise(e=>n.T=e),n.N=new Promise(e=>n.O=e),e["s-p"]=[],e["s-rc"]=[],h(e,n,t.q),le.set(e,n)},ie=(e,t)=>t in e,ce=e=>console.error(e),ae=new Map,fe=e=>{const t=e.P.replace(/-/g,"_"),n=e.B,l=ae.get(n);return l?l[t]:__sc_import_components(`./${n}.entry.js`).then(e=>(ae.set(n,e),e[t]),ce)},ue=new Map,me=[],de=[],pe=[],$e=(e,t)=>n=>{e.push(n),i||(i=!0,t&&4&u.t?ye(we):u.raf(we))},he=(e,t)=>{let n=0,l=0;for(;n<e.length&&(l=performance.now())<t;)try{e[n++](l)}catch(o){ce(o)}n===e.length?e.length=0:0!==n&&e.splice(0,n)},we=()=>{r++,(e=>{for(let n=0;n<e.length;n++)try{e[n](performance.now())}catch(t){ce(t)}e.length=0})(me);const e=2==(6&u.t)?performance.now()+10*Math.ceil(r*(1/22)):1/0;he(de,e),he(pe,e),de.length>0&&(pe.push(...de),de.length=0),(i=me.length+de.length+pe.length>0)?u.raf(we):r=0},ye=e=>d().then(e),be=$e(me,!1),_e=$e(de,!0),ve=()=>a&&a.supports&&a.supports("color","var(--c)")?d():__sc_import_components("./css-shim-260ce44f.js").then(()=>(u.H=c.__cssshim)?(!1).i():0),ge=()=>{u.H=c.__cssshim;const e=Array.from(f.querySelectorAll("script")).find(e=>new RegExp("/components(\\.esm)?\\.js($|\\?|#)").test(e.src)||"components"===e.getAttribute("data-stencil-namespace")),t={};return"onbeforeload"in e&&!history.scrollRestoration?{then(){}}:(t.resourcesUrl=new URL(".",new URL(e.getAttribute("data-resources-url")||e.src,c.location.href)).href,je(t.resourcesUrl,e),c.customElements?d(t):__sc_import_components("./dom-b7f433c1.js").then(()=>t))},je=(e,t)=>{const n=`__sc_import_${"components".replace(/\s|-/g,"_")}`;try{c[n]=new Function("w",`return import(w);//${Math.random()}`)}catch(l){const o=new Map;c[n]=l=>{const s=new URL(l,e).href;let r=o.get(s);if(!r){const e=f.createElement("script");e.type="module",e.crossOrigin=t.crossOrigin,e.src=URL.createObjectURL(new Blob([`import * as m from '${s}'; window.${n}.m = m;`],{type:"application/javascript"})),r=new Promise(t=>{e.onload=()=>{t(c[n].m),e.remove()}}),o.set(s,r),f.head.appendChild(e)}return r}}};export{$ as C,ve as a,Z as b,te as c,ne as d,ee as g,j as h,ge as p,se as r};