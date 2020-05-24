import { Component, Emit, Prop, Vue } from 'vue-property-decorator';
import DatePicker from '@/components/date-picker.component.vue';
import TimePicker from '@/components/time-picker.component.vue';

@Component({
    components: {
        DatePicker,
        TimePicker
    }
})
export default class DynamicInputComponent extends Vue {
    @Prop({ type: Object, required: true })
    options!: InputOptions;

    @Prop({ type: [String, Number], required: true, default: '' })
    value!: string|number;

    @Emit('input')
    onInput(value: string) {
        if (this.options.type === 'number') {
            const parsedNumber = parseFloat(value);
            return isNaN(parsedNumber) ? value : parsedNumber;
        }
        return value;
    }
}

export interface InputOptions {
    label: string;
    key: string;
    defaultValue?: string|number;
    type?: 'text' | 'number' | 'date';
    rules?: ((value: string) => (boolean | string))[];
    maxLength?: number;
    required?: boolean;
    prependInnerIcon?: string | boolean;
}
