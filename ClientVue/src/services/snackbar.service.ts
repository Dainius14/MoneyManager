import Vue from 'vue';
import SnackbarComponent from '../components/snackbar.vue';

enum Icon {
    Success = 'mdi-check',
    Error = 'mdi-alert',
}
enum Color {
    Success = 'success',
    Error = 'error',
    Info = 'info',
}

export interface ToastOptions {
    icon?: Icon;
    color?: Color;
    timeout?: number;
    closable?: boolean;
    multiline?: boolean;
}

class ToastServiceImpl {
    private readonly defaultOptions: ToastOptions = {
        icon: undefined,
        color: Color.Info,
        timeout: 3000,
        closable: true,
        multiline: false,
    };

    private _toastComponent?: SnackbarComponent;

    get toastComponent(): any {
        if (!this._toastComponent) {
            this._toastComponent = this.createToastComponent();
        }
    
        return this._toastComponent;
    }

    private createToastComponent(): any {
        const component = new Vue(SnackbarComponent);
        document.querySelector('#app')!.appendChild(component.$mount().$el);
        return component;
    }
    
    
    show(text: string, options?: ToastOptions) {
        this.toastComponent.show(text, { ...this.defaultOptions, ...options });
    }

    success(text: string) {
        this.show(text, { icon: Icon.Success, color: Color.Success });
    }

    error(text: string) {
        this.show(text, { icon: Icon.Error, color: Color.Error });

    }
    
    close() {
        this.toastComponent.close();
    }
}

export const ToastService = new ToastServiceImpl();
