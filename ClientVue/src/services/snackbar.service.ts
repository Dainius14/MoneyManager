import Vue from 'vue';
import Snackbar from './Snackbar.vue';

const colors = ['success', 'info', 'error'];

export interface ToastOptions {
    icon?: string;
    color?: string;
    timeout?: number;
    dismissable?: boolean;
}

class ToastServiceImpl {
    private readonly defaultOptions: ToastOptions = {
        icon: '',
        color: 'success',
        timeout: 3000,
        dismissable: true,
    };

    private _toastComponent?: Snackbar;

    get toastComponent(): any {
        if (!this._toastComponent) {
            this._toastComponent = this.createToastComponent();
        }
    
        return this._toastComponent;
    }

    private createToastComponent(): any {
        const component = new Vue(Snackbar);
        document.querySelector('#app')!.appendChild(component.$mount().$el);
        return component;
    }
    
    
    show(text: string, options?: ToastOptions) {
        this.toastComponent.show(text, { ...this.defaultOptions, ...options });
    }
    
    
    close() {
        this.toastComponent.close();
    }
}

export const ToastService = new ToastServiceImpl();
