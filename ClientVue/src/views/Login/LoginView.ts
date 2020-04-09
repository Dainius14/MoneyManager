import { Component, Vue } from 'vue-property-decorator';
import router, { Routes } from '@/router';
import { UserModule } from '@/store/modules/users-module.store';
import { ToastService } from '@/services/snackbar.service';

@Component({})
export default class LoginView extends Vue {
    email: string = 'dd@d.lt';
    password: string = 'ayylmao';
    
    isFormValid: boolean = false;

    private emailRegex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/i;

    emailRules = [
        (value: string) => this.validateEmail(value) || 'Invalid email'
    ];

    passwordRules = [
        (value: string) => !!value || 'Password is required'
    ];


    async onLoginClick() {
        try {
            await UserModule.login({ email: this.email, password: this.password });
            router.push(Routes.Root.path);
        }
        catch (e) {
            ToastService.show(e, { color: 'error' });
        }
    }

    private validateEmail(email: string): boolean {
        return this.emailRegex.test(email);
    }
}
