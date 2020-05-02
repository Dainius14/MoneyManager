import { Component, Vue } from 'vue-property-decorator';
import router, { Routes } from '@/router';
import { UserModule } from '@/store/modules/users-module.store';
import { ToastService } from '@/services/snackbar.service';
import { email, notEmpty } from '@/utils/rules';

@Component({})
export default class LoginView extends Vue {
    email: string = '';
    password: string = '';
    
    isFormValid: boolean = false;
    loggingIn: boolean = false;

    emailRules = [
        notEmpty('Valid email is required'),
        email('Valid email is required')
    ];

    passwordRules = [
        notEmpty('Password is required')
    ];


    async onLogin() {
        this.loggingIn = true;
        try {
            await UserModule.login({ email: this.email, password: this.password });
            await router.push(Routes.Root.path);
        }
        catch (e) {
            const ex = e as Error;
            ToastService.error(ex.message);
        }
        this.loggingIn = false;
    }
}
