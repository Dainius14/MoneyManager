// import _Vue from 'vue';

// export default {
//     install(Vue: typeof _Vue, options?: { appendToSelector: string; component: typeof _Vue }) {
//         const defaultOptions = {
//             appendToSelector: '#app'
//         };
//         const componentConstructor = Vue.extend(options?.component)
        
//         function toast() {
//             // options = options ?? {};
//             // options.message = msg;

//             let toast = CACHE[options.id] || (CACHE[options.id] = new CONSTRUCTOR)
//             if (!toast.$el) {
//                 let vm = toast.$mount()
//                 document.querySelector(options?.appendToSelector || '#app')?.appendChild(vm.$el)
//             }
//             // toast.queue.push(options)
//         }

//         Vue.prototype.$toast = toast;
//     }
// }
