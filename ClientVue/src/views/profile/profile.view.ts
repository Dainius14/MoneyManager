import { Component, Vue } from 'vue-property-decorator';
import router, { Routes } from '@/router';
import { UserModule } from '@/store/modules/users-module.store';
import { ToastService } from '@/services/snackbar.service';
import { email, notEmpty } from '@/utils/rules';

@Component({})
export default class ProfileView extends Vue {
    isChangeEmailFormValid: boolean = false;
    email: string = '';
    changingEmail: boolean = false;
    emailRules = [
        notEmpty('Valid email is required'),
        email('Valid email is required'),
    ];



    changingPassword: boolean = false;
    isChangePasswordFormValid: boolean = false;
    currentPassword: string = '';
    newPassword: string = '';
    repeatNewPassword: string = '';

    passwordRules = [
        notEmpty('Password is required')
    ];

    newPasswordRules = [
        notEmpty('New password is required'),
        () => this.passwordsMatch() || 'Password do not match'
    ];

    mounted() {
        this.email = UserModule.currentUser.email;
    }

    private passwordsMatch() {
        return this.newPassword === this.repeatNewPassword;
    }

    async onChangeEmail() {
        this.changingEmail = true;
        try {
            await UserModule.changeEmail(this.email);
            await router.push(Routes.Login);
            ToastService.success('Email changed successfully. Please login again');
        }
        catch (e) {
            const ex = e as Error;
            ToastService.error(ex.message);
        }
        this.changingEmail = false;
    }

    async onChangePassword() {
        this.changingPassword = true;
        try {
            await UserModule.changePassword({
                currentPassword: this.currentPassword,
                newPassword: this.newPassword
            });
            await router.push(Routes.Login);
            ToastService.success('Password changed successfully. Please login again');
        }
        catch (e) {
            const ex = e as Error;
            ToastService.error(ex.message);
        }
        this.changingPassword = false;
    }
}
