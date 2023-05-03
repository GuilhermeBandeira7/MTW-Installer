using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


namespace UtilsCore
{
    public class MessageController
    {
        #region SINGLETON

        private static MessageController instance = null;

        private MessageController()
        {

        }


        public static MessageController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageController();
                }
                return instance;
            }
        }




        #endregion

        public bool CheckIfIsMessage(List<byte> message)
        {
            if (message[0] == 0x5A)
                return true;
            return false;
        }

        public MessageModel CreateMessage(Dictionary<VARIABLES, List<byte>> values, BOARDS board, MESSAGE_TYPE type)
        {
            MessageModel messageModel = new MessageModel();
            messageModel.preamble = 0x5A;
            messageModel.id = (byte)board;
            messageModel.version = 0x20;
            messageModel.command = (byte)type;
            messageModel.numberOfVariables = (short)values.Keys.Count;

            int messageSize = 0;

            foreach (VARIABLES var in values.Keys)
            {
                messageSize += GetVarSize(var);
            }

            messageModel.sizeOfData = (short)messageSize;
            messageModel.message = values;

            return messageModel;
        }

        public MessageModel CreateMessage(Dictionary<VARIABLES, List<byte>> values, int board, MESSAGE_TYPE type)
        {
            MessageModel messageModel = new MessageModel();
            messageModel.preamble = 0x5A;
            messageModel.id = (byte)board;
            messageModel.version = 0x20;
            messageModel.command = (byte)type;
            messageModel.numberOfVariables = (short)values.Keys.Count;

            int messageSize = 0;

            foreach (VARIABLES var in values.Keys)
            {
                messageSize += GetVarSize(var);
            }

            messageModel.sizeOfData = (short)messageSize;
            messageModel.message = values;

            return messageModel;
        }


        public BOARD ConvertMessageToBoard(List<byte> message)
        {
            BOARD board = new BOARD();
            MessageModel messageModel = DecodeMessage(message);
            BOARDS id = (BOARDS)messageModel.id;

            switch (id)
            {
                case BOARDS.CPU:
                    board = new CPU();
                    ConvertVariableToClass(board, messageModel.message);
                    return board;
                case BOARDS.EB:
                    board = new EB();
                    ConvertVariableToClass(board, messageModel.message);
                    return board;
                case BOARDS.AC:
                    board = new AC();
                    ConvertVariableToClass(board, messageModel.message);
                    return board;

            }



            return board;
        }

        private void ConvertVariableToClass(object board, Dictionary<VARIABLES, List<byte>> message)
        {
            Type objType = board.GetType();
            FieldInfo[] fields = objType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            try
            {
                foreach (var field in fields)
                {
                    foreach (KeyValuePair<VARIABLES, List<byte>> variable in message)
                    {
                        if (Enum.GetName(variable.Key) == field.Name)
                        {
                            switch (Type.GetTypeCode(field.GetValue(board).GetType()))
                            {
                                case TypeCode.Int32:
                                    int intValue = 0;
                                    for (int counter = 0; counter < variable.Value.Count; counter++)
                                    {
                                        intValue += ((variable.Value[counter]) << (8 * (variable.Value.Count - counter - 1)));
                                    }

                                    field.SetValue(board, intValue);

                                    break;
                                case TypeCode.Boolean:
                                    bool boolValue = Convert.ToBoolean(variable.Value[0]);
                                    field.SetValue(board, boolValue);

                                    break;



                            }

                        }
                    }
                }

            }
            catch
            {

            }

        }

        public MessageModel DecodeMessage(List<byte> message)
        {
            Dictionary<VARIABLES, List<byte>> values = new Dictionary<VARIABLES, List<byte>>();
            MessageModel messageModel = new MessageModel();
            messageModel.preamble = message[0];
            messageModel.id = message[1];
            messageModel.version = message[2];
            messageModel.command = message[3];
            messageModel.numberOfVariables = (short)((message[4] << 8) + message[5]);
            messageModel.sizeOfData = (short)((message[6] << 8) + message[7]);


            int startVarValue = 8;
            int startDataValue = startVarValue + (messageModel.numberOfVariables * 2);

            int variableCounter = 0;
            for (int counter = 0; counter < messageModel.numberOfVariables * 2; counter = counter = counter + 2)
            {
                List<byte> value = new List<byte>();
                VARIABLES variable = (VARIABLES)Enum.ToObject(typeof(VARIABLES), (short)((message[startVarValue + counter] << 8) + message[startVarValue + 1 + counter]));
                int varSize = GetVarSize(variable);

                for (int dataCounter = 0; dataCounter < varSize; dataCounter++)
                {                                   
                    value.Add(message[startDataValue + variableCounter + dataCounter]);
                }
                variableCounter += varSize;
                values.Add(variable, value);


            }

            messageModel.message = values;
            return messageModel;
        }

        public byte[] EncodeMessage(MessageModel messageModel)
        {
            List<byte> message = new List<byte>();
            message.Add(messageModel.preamble);
            message.Add(messageModel.id);
            message.Add(messageModel.version);
            message.Add(messageModel.command);
            message.Add(GetMSB(messageModel.numberOfVariables));
            message.Add(GetLSB(messageModel.numberOfVariables));
            message.Add(GetMSB(messageModel.sizeOfData));
            message.Add(GetLSB(messageModel.sizeOfData));

            foreach (VARIABLES variable in messageModel.message.Keys)
            {
                message.Add(GetMSB((short)variable));
                message.Add(GetLSB((short)variable));
            }

            foreach (KeyValuePair<VARIABLES, List<byte>> value in messageModel.message)
            {
                for (int counter = GetVarSize(value.Key) - 1; counter >= 0; counter--)
                {
                    if (value.Value.Count > counter)
                    {
                        message.Add(value.Value[counter]);
                    }
                    else
                    {
                        message.Add(0x00);
                    }
                }
            }

            return message.ToArray();
        }

        public MessageModel AddValueToMessage(MessageModel message, VARIABLES variable, List<byte> value)
        {
            return message;
        }


        public byte GetMSB(short value)
        {
            return ((byte)((value >> 8) & 0xFF));
        }


        public byte GetLSB(short value)
        {
            return ((byte)(value & 0xFF));
        }

        public int GetVarSize(VARIABLES var)
        {
            switch (var)
            {
                case VARIABLES.SN:
                    return 7;
                case VARIABLES.RT_CPU:
                    return 2;
                case VARIABLES.RT_EXP1:
                    return 2;
                case VARIABLES.RT_EXP2:
                    return 2;
                case VARIABLES.RT_EXP3:
                    return 2;
                case VARIABLES.VAR_TE01:
                    return 1;
                case VARIABLES.TE01:
                    return 2;
                case VARIABLES.VAR_HU01:
                    return 1;
                case VARIABLES.HU01:
                    return 2;
                case VARIABLES.AU01_toggle:
                    return 1;
                case VARIABLES.AP01_port_in:
                    return 2;
                case VARIABLES.AP02_target_port:
                    return 2;
                case VARIABLES.IP_D:
                    return 4;
                case VARIABLES.IP_M:
                    return 4;
                case VARIABLES.IP_G:
                    return 4;
                case VARIABLES.IP_S:
                    return 4;
                case VARIABLES.IP_audio_target:
                    return 4;
                case VARIABLES.IP_Reset:
                    return 1;
                case VARIABLES.DEVICE_RESET:
                case VARIABLES.RESET_COUNTER:
                    return 1;
                case VARIABLES.RESET_PULSE:
                    return 1;
                case VARIABLES.RESET_LIMIT:
                    return 1;
                case VARIABLES.RESET_STATUS:
                    return 1;
                case VARIABLES.TUNNEL_MODE:
                    return 1;
                case VARIABLES.SR01:
                case VARIABLES.SR02:
                case VARIABLES.SR03:
                case VARIABLES.SR04:
                case VARIABLES.SR05:
                    return 512;
                case VARIABLES.SR01_MODE:
                case VARIABLES.SR01_RX_LEVEL:
                case VARIABLES.SR01_TX_LEVEL:
                case VARIABLES.SR01_PARITY_STATUS:
                case VARIABLES.SR01_PARITY_MODE:
                case VARIABLES.SR01_BAUDRATE:
                case VARIABLES.SR01_STATUS:
                case VARIABLES.SR02_MODE:
                case VARIABLES.SR02_RX_LEVEL:
                case VARIABLES.SR02_TX_LEVEL:
                case VARIABLES.SR02_PARITY_STATUS:
                case VARIABLES.SR02_PARITY_MODE:
                case VARIABLES.SR02_BAUDRATE:
                case VARIABLES.SR02_STATUS:
                case VARIABLES.SR03_MODE:
                case VARIABLES.SR03_RX_LEVEL:
                case VARIABLES.SR03_TX_LEVEL:
                case VARIABLES.SR03_PARITY_STATUS:
                case VARIABLES.SR03_PARITY_MODE:
                case VARIABLES.SR03_BAUDRATE:
                case VARIABLES.SR03_STATUS:
                case VARIABLES.SR04_MODE:
                case VARIABLES.SR04_RX_LEVEL:
                case VARIABLES.SR04_TX_LEVEL:
                case VARIABLES.SR04_PARITY_STATUS:
                case VARIABLES.SR04_PARITY_MODE:
                case VARIABLES.SR04_BAUDRATE:
                case VARIABLES.SR04_STATUS:
                case VARIABLES.SR05_MODE:
                case VARIABLES.SR05_RX_LEVEL:
                case VARIABLES.SR05_TX_LEVEL:
                case VARIABLES.SR05_PARITY_STATUS:
                case VARIABLES.SR05_PARITY_MODE:
                case VARIABLES.SR05_STATUS:
                case VARIABLES.SR05_BAUDRATE:
                    return 1;
                case VARIABLES.DO01:
                case VARIABLES.DO02:
                case VARIABLES.DO03:
                case VARIABLES.DO04:
                case VARIABLES.DO05:
                case VARIABLES.DO06:
                case VARIABLES.DO07:
                case VARIABLES.DO08:
                case VARIABLES.DO09:
                case VARIABLES.DO10:
                case VARIABLES.DO11:
                case VARIABLES.DO12:
                case VARIABLES.DO13:
                case VARIABLES.DO14:
                case VARIABLES.DO15:
                case VARIABLES.DO16:
                case VARIABLES.DO17:
                case VARIABLES.DO18:
                case VARIABLES.DO19:
                case VARIABLES.DO20:
                case VARIABLES.DO21:
                case VARIABLES.DO22:
                case VARIABLES.DO23:
                case VARIABLES.DO24:
                case VARIABLES.DO25:
                case VARIABLES.DO26:
                case VARIABLES.DO27:
                case VARIABLES.DO28:
                case VARIABLES.DO29:
                case VARIABLES.DO30:
                case VARIABLES.DO31:
                case VARIABLES.DO32:
                case VARIABLES.DO33:
                case VARIABLES.DO34:
                case VARIABLES.DO35:
                case VARIABLES.DO36:
                case VARIABLES.DO37:
                case VARIABLES.DO38:
                case VARIABLES.DO39:
                case VARIABLES.DO40:
                case VARIABLES.DO41:
                case VARIABLES.DO42:
                case VARIABLES.DO43:
                case VARIABLES.DO44:
                case VARIABLES.DO45:
                case VARIABLES.DO46:
                case VARIABLES.DO47:
                case VARIABLES.DO48:
                case VARIABLES.DO49:
                case VARIABLES.DO50:
                case VARIABLES.DO51:
                case VARIABLES.DO52:
                case VARIABLES.DO53:
                case VARIABLES.DO54:
                case VARIABLES.DO55:
                case VARIABLES.DO56:
                case VARIABLES.DO57:
                case VARIABLES.DO58:
                case VARIABLES.DO59:
                case VARIABLES.DO60:
                case VARIABLES.DO61:
                case VARIABLES.DO62:
                case VARIABLES.DO63:
                case VARIABLES.DO64:
                case VARIABLES.DI01:
                case VARIABLES.DI02:
                case VARIABLES.DI03:
                case VARIABLES.DI04:
                case VARIABLES.DI05:
                case VARIABLES.DI06:
                case VARIABLES.DI07:
                case VARIABLES.DI08:
                case VARIABLES.DI09:
                case VARIABLES.DI10:
                case VARIABLES.DI11:
                case VARIABLES.DI12:
                case VARIABLES.DI13:
                case VARIABLES.DI14:
                case VARIABLES.DI15:
                case VARIABLES.DI16:
                case VARIABLES.DI17:
                case VARIABLES.DI18:
                case VARIABLES.DI19:
                case VARIABLES.DI20:
                case VARIABLES.DI21:
                case VARIABLES.DI22:
                case VARIABLES.DI23:
                case VARIABLES.DI24:
                case VARIABLES.DI25:
                case VARIABLES.DI26:
                case VARIABLES.DI27:
                case VARIABLES.DI28:
                case VARIABLES.DI29:
                case VARIABLES.DI30:
                case VARIABLES.DI31:
                case VARIABLES.DI32:
                case VARIABLES.DI33:
                case VARIABLES.DI34:
                case VARIABLES.DI35:
                case VARIABLES.DI36:
                case VARIABLES.DI37:
                case VARIABLES.DI38:
                case VARIABLES.DI39:
                case VARIABLES.DI40:
                case VARIABLES.DI41:
                case VARIABLES.DI42:
                case VARIABLES.DI43:
                case VARIABLES.DI44:
                case VARIABLES.DI45:
                case VARIABLES.DI46:
                case VARIABLES.DI47:
                case VARIABLES.DI48:
                case VARIABLES.DI49:
                case VARIABLES.DI50:
                case VARIABLES.DI51:
                case VARIABLES.DI52:
                case VARIABLES.DI53:
                case VARIABLES.DI54:
                case VARIABLES.DI55:
                case VARIABLES.DI56:
                case VARIABLES.DI57:
                case VARIABLES.DI58:
                case VARIABLES.DI59:
                case VARIABLES.DI60:
                case VARIABLES.DI61:
                case VARIABLES.DI62:
                case VARIABLES.DI63:
                case VARIABLES.DI64:
                    return 1;
                case VARIABLES.VAR_DC01:
                case VARIABLES.VAR_DC02:
                case VARIABLES.VAR_DC03:
                case VARIABLES.VAR_DC04:
                case VARIABLES.VAR_DC05:
                case VARIABLES.VAR_DC06:
                case VARIABLES.VAR_DC07:
                case VARIABLES.VAR_DC08:
                case VARIABLES.VAR_DC09:
                case VARIABLES.VAR_DC10:
                case VARIABLES.VAR_DC11:
                case VARIABLES.VAR_DC12:
                case VARIABLES.VAR_DC13:
                case VARIABLES.VAR_DC14:
                case VARIABLES.VAR_DC15:
                case VARIABLES.VAR_DC16:
                case VARIABLES.VAR_DC17:
                case VARIABLES.VAR_DC18:
                case VARIABLES.VAR_DC19:
                case VARIABLES.VAR_DC20:
                case VARIABLES.VAR_DC21:
                case VARIABLES.VAR_DC22:
                case VARIABLES.VAR_DC23:
                case VARIABLES.VAR_DC24:
                case VARIABLES.VAR_DC25:
                case VARIABLES.VAR_DC26:
                case VARIABLES.VAR_DC27:
                case VARIABLES.VAR_DC28:
                case VARIABLES.VAR_DC29:
                case VARIABLES.VAR_DC30:
                case VARIABLES.VAR_DC31:
                case VARIABLES.VAR_DC32:
                case VARIABLES.VAR_DC33:
                case VARIABLES.VAR_DC34:
                case VARIABLES.VAR_DC35:
                case VARIABLES.VAR_DC36:
                case VARIABLES.VAR_DC37:
                case VARIABLES.VAR_DC38:
                case VARIABLES.VAR_DC39:
                case VARIABLES.VAR_DC40:
                case VARIABLES.VAR_DC41:
                case VARIABLES.VAR_DC42:
                case VARIABLES.VAR_DC43:
                case VARIABLES.VAR_DC44:
                case VARIABLES.VAR_DC45:
                case VARIABLES.VAR_DC46:
                case VARIABLES.VAR_DC47:
                case VARIABLES.VAR_DC48:
                case VARIABLES.VAR_DC49:
                case VARIABLES.VAR_DC50:
                case VARIABLES.VAR_DC51:
                case VARIABLES.VAR_DC52:
                case VARIABLES.VAR_DC53:
                case VARIABLES.VAR_DC54:
                case VARIABLES.VAR_DC55:
                case VARIABLES.VAR_DC56:
                case VARIABLES.VAR_DC57:
                case VARIABLES.VAR_DC58:
                case VARIABLES.VAR_DC59:
                case VARIABLES.VAR_DC60:
                case VARIABLES.VAR_DC61:
                case VARIABLES.VAR_DC62:
                case VARIABLES.VAR_DC63:
                case VARIABLES.VAR_DC64:
                case VARIABLES.VAR_AC01:
                case VARIABLES.VAR_AC02:
                case VARIABLES.VAR_AC03:
                case VARIABLES.VAR_AC04:
                case VARIABLES.VAR_AC05:
                case VARIABLES.VAR_AC06:
                case VARIABLES.VAR_AC07:
                case VARIABLES.VAR_AC08:
                case VARIABLES.VAR_AC09:
                case VARIABLES.VAR_AC10:
                case VARIABLES.VAR_AC11:
                case VARIABLES.VAR_AC12:
                case VARIABLES.VAR_AC13:
                case VARIABLES.VAR_AC14:
                case VARIABLES.VAR_AC15:
                case VARIABLES.VAR_AC16:
                case VARIABLES.VAR_AC17:
                case VARIABLES.VAR_AC18:
                case VARIABLES.VAR_AC19:
                case VARIABLES.VAR_AC20:
                case VARIABLES.VAR_AC21:
                case VARIABLES.VAR_AC22:
                case VARIABLES.VAR_AC23:
                case VARIABLES.VAR_AC24:
                case VARIABLES.VAR_AC25:
                case VARIABLES.VAR_AC26:
                case VARIABLES.VAR_AC27:
                case VARIABLES.VAR_AC28:
                case VARIABLES.VAR_AC29:
                case VARIABLES.VAR_AC30:
                case VARIABLES.VAR_AC31:
                case VARIABLES.VAR_AC32:
                    return 1;
                case VARIABLES.CAL_DC01:
                case VARIABLES.DC01:
                case VARIABLES.CAL_DC02:
                case VARIABLES.DC02:
                case VARIABLES.CAL_DC03:
                case VARIABLES.DC03:
                case VARIABLES.CAL_DC04:
                case VARIABLES.DC04:
                case VARIABLES.CAL_DC05:
                case VARIABLES.DC05:
                case VARIABLES.CAL_DC06:
                case VARIABLES.DC06:
                case VARIABLES.CAL_DC07:
                case VARIABLES.DC07:
                case VARIABLES.CAL_DC08:
                case VARIABLES.DC08:
                case VARIABLES.CAL_DC09:
                case VARIABLES.DC09:
                case VARIABLES.CAL_DC10:
                case VARIABLES.DC10:
                case VARIABLES.CAL_DC11:
                case VARIABLES.DC11:
                case VARIABLES.CAL_DC12:
                case VARIABLES.DC12:
                case VARIABLES.CAL_DC13:
                case VARIABLES.DC13:
                case VARIABLES.CAL_DC14:
                case VARIABLES.DC14:
                case VARIABLES.CAL_DC15:
                case VARIABLES.DC15:
                case VARIABLES.CAL_DC16:
                case VARIABLES.DC16:
                case VARIABLES.CAL_DC17:
                case VARIABLES.DC17:
                case VARIABLES.CAL_DC18:
                case VARIABLES.DC18:
                case VARIABLES.CAL_DC19:
                case VARIABLES.DC19:
                case VARIABLES.CAL_DC20:
                case VARIABLES.DC20:
                case VARIABLES.CAL_DC21:
                case VARIABLES.DC21:
                case VARIABLES.CAL_DC22:
                case VARIABLES.DC22:
                case VARIABLES.CAL_DC23:
                case VARIABLES.DC23:
                case VARIABLES.CAL_DC24:
                case VARIABLES.DC24:
                case VARIABLES.CAL_DC25:
                case VARIABLES.DC25:
                case VARIABLES.CAL_DC26:
                case VARIABLES.DC26:
                case VARIABLES.CAL_DC27:
                case VARIABLES.DC27:
                case VARIABLES.CAL_DC28:
                case VARIABLES.DC28:
                case VARIABLES.CAL_DC29:
                case VARIABLES.DC29:
                case VARIABLES.CAL_DC30:
                case VARIABLES.DC30:
                case VARIABLES.CAL_DC31:
                case VARIABLES.DC31:
                case VARIABLES.CAL_DC32:
                case VARIABLES.DC32:
                case VARIABLES.CAL_DC33:
                case VARIABLES.DC33:
                case VARIABLES.CAL_DC34:
                case VARIABLES.DC34:
                case VARIABLES.CAL_DC35:
                case VARIABLES.DC35:
                case VARIABLES.CAL_DC36:
                case VARIABLES.DC36:
                case VARIABLES.CAL_DC37:
                case VARIABLES.DC37:
                case VARIABLES.CAL_DC38:
                case VARIABLES.DC38:
                case VARIABLES.CAL_DC39:
                case VARIABLES.DC39:
                case VARIABLES.CAL_DC40:
                case VARIABLES.DC40:
                case VARIABLES.CAL_DC41:
                case VARIABLES.DC41:
                case VARIABLES.CAL_DC42:
                case VARIABLES.DC42:
                case VARIABLES.CAL_DC43:
                case VARIABLES.DC43:
                case VARIABLES.CAL_DC44:
                case VARIABLES.DC44:
                case VARIABLES.CAL_DC45:
                case VARIABLES.DC45:
                case VARIABLES.CAL_DC46:
                case VARIABLES.DC46:
                case VARIABLES.CAL_DC47:
                case VARIABLES.DC47:
                case VARIABLES.CAL_DC48:
                case VARIABLES.DC48:
                case VARIABLES.CAL_DC49:
                case VARIABLES.DC49:
                case VARIABLES.CAL_DC50:
                case VARIABLES.DC50:
                case VARIABLES.CAL_DC51:
                case VARIABLES.DC51:
                case VARIABLES.CAL_DC52:
                case VARIABLES.DC52:
                case VARIABLES.CAL_DC53:
                case VARIABLES.DC53:
                case VARIABLES.CAL_DC54:
                case VARIABLES.DC54:
                case VARIABLES.CAL_DC55:
                case VARIABLES.DC55:
                case VARIABLES.CAL_DC56:
                case VARIABLES.DC56:
                case VARIABLES.CAL_DC57:
                case VARIABLES.DC57:
                case VARIABLES.CAL_DC58:
                case VARIABLES.DC58:
                case VARIABLES.CAL_DC59:
                case VARIABLES.DC59:
                case VARIABLES.CAL_DC60:
                case VARIABLES.DC60:
                case VARIABLES.CAL_DC61:
                case VARIABLES.DC61:
                case VARIABLES.CAL_DC62:
                case VARIABLES.DC62:
                case VARIABLES.CAL_DC63:
                case VARIABLES.DC63:
                case VARIABLES.CAL_DC64:
                case VARIABLES.DC64:
                case VARIABLES.CAL50_AC01:
                case VARIABLES.CAL127_AC01:
                case VARIABLES.CAL220_AC01:
                case VARIABLES.AC01:
                case VARIABLES.CAL50_AC02:
                case VARIABLES.CAL127_AC02:
                case VARIABLES.CAL220_AC02:
                case VARIABLES.AC02:
                case VARIABLES.CAL50_AC03:
                case VARIABLES.CAL127_AC03:
                case VARIABLES.CAL220_AC03:
                case VARIABLES.AC03:
                case VARIABLES.CAL50_AC04:
                case VARIABLES.CAL127_AC04:
                case VARIABLES.CAL220_AC04:
                case VARIABLES.AC04:
                case VARIABLES.CAL50_AC05:
                case VARIABLES.CAL127_AC05:
                case VARIABLES.CAL220_AC05:
                case VARIABLES.AC05:
                case VARIABLES.CAL50_AC06:
                case VARIABLES.CAL127_AC06:
                case VARIABLES.CAL220_AC06:
                case VARIABLES.AC06:
                case VARIABLES.CAL50_AC07:
                case VARIABLES.CAL127_AC07:
                case VARIABLES.CAL220_AC07:
                case VARIABLES.AC07:
                case VARIABLES.CAL50_AC08:
                case VARIABLES.CAL127_AC08:
                case VARIABLES.CAL220_AC08:
                case VARIABLES.AC08:
                case VARIABLES.CAL50_AC09:
                case VARIABLES.CAL127_AC09:
                case VARIABLES.CAL220_AC09:
                case VARIABLES.AC09:
                case VARIABLES.CAL50_AC10:
                case VARIABLES.CAL127_AC10:
                case VARIABLES.CAL220_AC10:
                case VARIABLES.AC10:
                case VARIABLES.CAL50_AC11:
                case VARIABLES.CAL127_AC11:
                case VARIABLES.CAL220_AC11:
                case VARIABLES.AC11:
                case VARIABLES.CAL50_AC12:
                case VARIABLES.CAL127_AC12:
                case VARIABLES.CAL220_AC12:
                case VARIABLES.AC12:
                case VARIABLES.CAL50_AC13:
                case VARIABLES.CAL127_AC13:
                case VARIABLES.CAL220_AC13:
                case VARIABLES.AC13:
                case VARIABLES.CAL50_AC14:
                case VARIABLES.CAL127_AC14:
                case VARIABLES.CAL220_AC14:
                case VARIABLES.AC14:
                case VARIABLES.CAL50_AC15:
                case VARIABLES.CAL127_AC15:
                case VARIABLES.CAL220_AC15:
                case VARIABLES.AC15:
                case VARIABLES.CAL50_AC16:
                case VARIABLES.CAL127_AC16:
                case VARIABLES.CAL220_AC16:
                case VARIABLES.AC16:
                case VARIABLES.CAL50_AC17:
                case VARIABLES.CAL127_AC17:
                case VARIABLES.CAL220_AC17:
                case VARIABLES.AC17:
                case VARIABLES.CAL50_AC18:
                case VARIABLES.CAL127_AC18:
                case VARIABLES.CAL220_AC18:
                case VARIABLES.AC18:
                case VARIABLES.CAL50_AC19:
                case VARIABLES.CAL127_AC19:
                case VARIABLES.CAL220_AC19:
                case VARIABLES.AC19:
                case VARIABLES.CAL50_AC20:
                case VARIABLES.CAL127_AC20:
                case VARIABLES.CAL220_AC20:
                case VARIABLES.AC20:
                case VARIABLES.CAL50_AC21:
                case VARIABLES.CAL127_AC21:
                case VARIABLES.CAL220_AC21:
                case VARIABLES.AC21:
                case VARIABLES.CAL50_AC22:
                case VARIABLES.CAL127_AC22:
                case VARIABLES.CAL220_AC22:
                case VARIABLES.AC22:
                case VARIABLES.CAL50_AC23:
                case VARIABLES.CAL127_AC23:
                case VARIABLES.CAL220_AC23:
                case VARIABLES.AC23:
                case VARIABLES.CAL50_AC24:
                case VARIABLES.CAL127_AC24:
                case VARIABLES.CAL220_AC24:
                case VARIABLES.AC24:
                case VARIABLES.CAL50_AC25:
                case VARIABLES.CAL127_AC25:
                case VARIABLES.CAL220_AC25:
                case VARIABLES.AC25:
                case VARIABLES.CAL50_AC26:
                case VARIABLES.CAL127_AC26:
                case VARIABLES.CAL220_AC26:
                case VARIABLES.AC26:
                case VARIABLES.CAL50_AC27:
                case VARIABLES.CAL127_AC27:
                case VARIABLES.CAL220_AC27:
                case VARIABLES.AC27:
                case VARIABLES.CAL50_AC28:
                case VARIABLES.CAL127_AC28:
                case VARIABLES.CAL220_AC28:
                case VARIABLES.AC28:
                case VARIABLES.CAL50_AC29:
                case VARIABLES.CAL127_AC29:
                case VARIABLES.CAL220_AC29:
                case VARIABLES.AC29:
                case VARIABLES.CAL50_AC30:
                case VARIABLES.CAL127_AC30:
                case VARIABLES.CAL220_AC30:
                case VARIABLES.AC30:
                case VARIABLES.CAL50_AC31:
                case VARIABLES.CAL127_AC31:
                case VARIABLES.CAL220_AC31:
                case VARIABLES.AC31:
                case VARIABLES.CAL50_AC32:
                case VARIABLES.CAL127_AC32:
                case VARIABLES.CAL220_AC32:
                case VARIABLES.AC32:
                    return 2;

                case VARIABLES.VAR_PH01:
                case VARIABLES.VAR_FQ01:
                case VARIABLES.VAR_PH02:
                case VARIABLES.VAR_FQ02:
                case VARIABLES.VAR_PH03:
                case VARIABLES.VAR_FQ03:
                    return 1;

                case VARIABLES.PH01:
                case VARIABLES.FQ01:
                case VARIABLES.PH02:
                case VARIABLES.FQ02:
                case VARIABLES.PH03:
                case VARIABLES.FQ03:
                    return 2;

                default:
                    return 0;
            }
        }

        #region AUXCODEFUNTIONS

        public byte[] CreateQueryByBoard(BOARDS board, int slot)
        {
            if(board == BOARDS.CPU)
            {
                Dictionary<VARIABLES, List<byte>> values = new Dictionary<VARIABLES, List<byte>>();
                values.Add(VARIABLES.DC01, new List<byte>() { 0x00, 0x00 });
                values.Add(VARIABLES.DC02, new List<byte>() { 0x00, 0x00 });
                values.Add(VARIABLES.DO01, new List<byte>() { 0x00 });
                values.Add(VARIABLES.DI01, new List<byte>() { 0x00 });
                values.Add(VARIABLES.TE01, new List<byte>() { 0x00, 0x00 });
                values.Add(VARIABLES.HU01, new List<byte>() { 0x00, 0x00 });

                return EncodeMessage(CreateMessage(values, board, MESSAGE_TYPE.QUERY));
            }
            else if (board == BOARDS.EB)
            {
                Dictionary<VARIABLES, List<byte>> values = new Dictionary<VARIABLES, List<byte>>();
                values.Add(VARIABLES.DC01, new List<byte>() { 0x00, 0x00 });
                values.Add(VARIABLES.DC02, new List<byte>() { 0x00, 0x00 });
                values.Add(VARIABLES.DO01, new List<byte>() { 0x00 });
                values.Add(VARIABLES.DI01, new List<byte>() { 0x00 });
                values.Add(VARIABLES.DO02, new List<byte>() { 0x00 });
                values.Add(VARIABLES.DI02, new List<byte>() { 0x00 });
                values.Add(VARIABLES.DO03, new List<byte>() { 0x00 });
                values.Add(VARIABLES.DI03, new List<byte>() { 0x00 });

                return EncodeMessage(CreateMessage(values, board, MESSAGE_TYPE.QUERY));
            }
            else if (board == BOARDS.AC)
            {
                Dictionary<VARIABLES, List<byte>> values = new Dictionary<VARIABLES, List<byte>>();
                values.Add(VARIABLES.AC01, new List<byte>() { 0x00, 0x00 });
                values.Add(VARIABLES.AC02, new List<byte>() { 0x00, 0x00 });
                values.Add(VARIABLES.AC03, new List<byte>() { 0x00, 0x00 });

                return EncodeMessage(CreateMessage(values, slot, MESSAGE_TYPE.QUERY));
            }


            return new byte[] { };
        }

        #endregion

    }
}
